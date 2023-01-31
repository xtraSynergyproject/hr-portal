using Microsoft.VisualBasic;
using Synergy.App.Business.Interface.BusinessScript.Task.General.EGov;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.EGov
{
    public class TaskGeneralEGovPreScript : ITaskGeneralEGovPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> ChangeBillCollectorAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["assigneeForBillCollector"] != null)
            {
                var assigneeId = rowdata["assigneeForBillCollector"].ToString();
                if (assigneeId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = assigneeId;
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> ChangeFieldInspectorAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["assigneeForFieldInspector"] != null)
            {
                var assigneeId = rowdata["assigneeForFieldInspector"].ToString();
                if (assigneeId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = assigneeId;
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> ChangeRevenueOfficerAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["assigneeFoRevenueOfficer"] != null)
            {
                var assigneeId = rowdata["assigneeFoRevenueOfficer"].ToString();
                if (assigneeId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = assigneeId;
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> ChangePaymentGatewayAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            //var assigneeId = rowdata["OwnerUserId"].ToString();
            var assigneeId = "";
            if (serviceData.IsNotNull())
            {
                viewModel.AssignedToUserId = serviceData.OwnerUserId;
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<TaskTemplateViewModel>> ChangeApproverAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["assigneeForFinalApprover"] != null)
            {
                var assigneeId = rowdata["assigneeForFinalApprover"].ToString();
                if (assigneeId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = assigneeId;
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> ChangeAssigneeToServiceOwner(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            //var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            //var assigneeId = Convert.ToString(rowdata["OwnerUserId"]);
            //if (assigneeId.IsNotNullAndNotEmpty())
            //{
            //    viewModel.AssignedToUserId = assigneeId;
            //}
            if (serviceData.IsNotNull())
            {
                viewModel.AssignedToUserId = serviceData.OwnerUserId;
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeToServiceOwner(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            //var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            //var assigneeId = Convert.ToString(rowdata["OwnerUserId"]);
            //if (assigneeId.IsNotNullAndNotEmpty())
            //{
            //    viewModel.AssignedToUserId = assigneeId;
            //}
            if (serviceData.IsNotNull())
            {
                viewModel.AssignedToUserId = serviceData.OwnerUserId;
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeToConsumer(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _smartcityBusiness = sp.GetService<ISmartCityBusiness>();
            //var serviceTempModel = new ServiceTemplateViewModel
            //{
            //    ServiceId = viewModel.ParentServiceId
            //};
            //var serviceData = await _serviceBusiness.GetServiceDetails(serviceTempModel);
            //if (serviceData.IsNotNull())
            //{
            //    var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(serviceData.Json);
            //    var consumerId = Convert.ToString(rowdata["ConsumerId"]);
            //    if (consumerId.IsNotNullAndNotEmpty())
            //    {
            //        string consumerUserId = await _smartcityBusiness.GetJSCConsumerUserId(consumerId);
            //        if (consumerUserId.IsNotNullAndNotEmpty())
            //        {
            //            viewModel.AssignedToUserId = consumerUserId;
            //        }
            //    }
            //}

            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["ConsumerId"] != null)
            {
                var consumerId = Convert.ToString(rowdata["ConsumerId"]);
                string consumerUserId = await _smartcityBusiness.GetJSCConsumerUserId(consumerId);
                if (consumerUserId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = consumerUserId;
                }
            }
            //if (serviceData.IsNotNull())
            //{
            //    viewModel.AssignedToUserId = serviceData.OwnerUserId;
            //}
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeForDeliverBinRequest(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();

            //var taskStatus = await _lovBusiness.GetSingleById(viewModel.TaskStatusId);

            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var garbageCollectorUserId = Convert.ToString(rowdata["DeliveryPerson"]);
            if (viewModel.TaskStatusCode == "TASK_STATUS_INPROGRESS")
            {
                if (garbageCollectorUserId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = garbageCollectorUserId;
                }
                var newDueDate = Convert.ToDateTime(rowdata["BookingFromDate"]);
                viewModel.DueDate = newDueDate.AddDays(1);
            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeForCompleteGarbageCollection(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();

            //var taskStatus = await _lovBusiness.GetSingleById(viewModel.TaskStatusId);

            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var garbageCollectorUserId = Convert.ToString(rowdata["DeliveryPerson"]);
            if (viewModel.TaskStatusCode == "TASK_STATUS_INPROGRESS")
            {
                if (garbageCollectorUserId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = garbageCollectorUserId;
                }
                var newDueDate = Convert.ToDateTime(rowdata["BookingToDate"]);
                viewModel.DueDate = newDueDate.AddDays(1);
            }
            else if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var dueDate = Convert.ToDateTime(rowdata["BookingToDate"]);
                if (dueDate <= DateTime.Today)
                {
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
                }
                else
                {
                    var errorList = new Dictionary<string, string>();
                    errorList.Add("Validate", "You can complete this task after " + dueDate.ToDefaultDateTimeFormat());
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);

        }

        public async Task<CommandResult<TaskTemplateViewModel>> JSCAssignTaskToPropertyOwner(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _portalBusiness = sp.GetService<IPortalBusiness>();
            var _userBusiness = sp.GetService<IUserBusiness>();
            var _smartCityBusiness = sp.GetService<ISmartCityBusiness>();

            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["ParcelId"] != null)
            {
                LoginViewModel model = new LoginViewModel();
                var mmi_id = Convert.ToString(rowdata["ParcelId"]);
                var propertyDetails = await _smartCityBusiness.GetParcelByPropertyId(mmi_id);
                if (propertyDetails.IsNotNull())
                {
                    var telephoneNo = propertyDetails.tel_no;

                    var user = await _userBusiness.GetSingle(x => x.Mobile == telephoneNo);

                    if (user == null)
                    {
                        if (telephoneNo.IsNullOrEmpty() || telephoneNo.Length != 10)
                        {
                            var errorList = new Dictionary<string, string>();
                            errorList.Add("Validate", "Mobile number is not valid");
                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                        }

                        user = new UserViewModel
                        {
                            Email = telephoneNo,
                            Mobile = telephoneNo,
                            UserName = telephoneNo,
                            EnableTwoFactorAuth = true,
                            Name = propertyDetails.own_name.IsNotNullAndNotEmpty() ? propertyDetails.own_name : telephoneNo

                        };
                        var userResult = await _userBusiness.Create(user);
                        if (userResult.IsSuccess)
                        {
                            var portalDetails = await _portalBusiness.GetSingle(x => x.Name == "JammuSmartCityCustomer");
                            if (portalDetails.IsNotNull())
                            {
                                await _userBusiness.Create<UserPortalViewModel, UserPortal>(new UserPortalViewModel
                                {
                                    PortalId = portalDetails.Id,
                                    UserId = user.Id,

                                });
                            }
                        }
                        if (user.Id.IsNotNullAndNotEmpty())
                        {
                            viewModel.AssignedToUserId = userResult.Item.Id;
                        }
                    }
                    else
                    {
                        if (user.Id.IsNotNullAndNotEmpty())
                        {
                            viewModel.AssignedToUserId = user.Id;
                        }
                    }
                    viewModel.TaskSubject = "Property Tax of : " + rowdata["ParcelId"];
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> SetAssigeeDetailsForJammuGrievance(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _smartCityBusiness = sp.GetService<ISmartCityBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);

            var ward = Convert.ToString(rowdata["Ward"]);
            var dept = Convert.ToString(rowdata["Department"]);
            if (ward.IsNotNull() && dept.IsNotNull())
            {
                var data = await _smartCityBusiness.GetGrievanceWorkflow(ward, dept);
                if (data.WorkflowLevelId.IsNotNull())
                {
                    var lov = await _lovBusiness.GetSingleById(data.WorkflowLevelId);
                    var assType = await _lovBusiness.GetSingleById(data.Level1AssignedToTypeId);
                    var level = viewModel.TemplateCode;
                    switch (level)
                    {
                        case "JSC_LEVEL_1":
                            viewModel.AssignedToTypeId = data.Level1AssignedToTypeId;
                            viewModel.AssignedToUserId = data.Level1AssignedToUserId;
                            viewModel.AssignedToTeamId = data.Level1AssignedToTeamId;
                            viewModel.AssignedToTypeCode = assType.Code;
                            break;
                        case "JSC_LEVEL_2":
                            if (lov.Code == "GR_WK_LEVEL2" || lov.Code == "GR_WK_LEVEL3" || lov.Code == "GR_WK_LEVEL4")
                            {
                                viewModel.AssignedToTypeId = data.Level2AssignedToTypeId;
                                viewModel.AssignedToUserId = data.Level2AssignedToUserId;
                                viewModel.AssignedToTeamId = data.Level2AssignedToTeamId;
                                viewModel.AssignedToTypeCode = assType.Code;
                            }
                            break;
                        case "JSC_LEVEL_3":
                            if (lov.Code == "GR_WK_LEVEL3" || lov.Code == "GR_WK_LEVEL4")
                            {
                                viewModel.AssignedToTypeId = data.Level3AssignedToTypeId;
                                viewModel.AssignedToUserId = data.Level3AssignedToUserId;
                                viewModel.AssignedToTeamId = data.Level3AssignedToTeamId;
                                viewModel.AssignedToTypeCode = assType.Code;
                            }
                            break;
                        case "JSC_LEVEL_4":
                            if (lov.Code == "GR_WK_LEVEL4")
                            {
                                viewModel.AssignedToTypeId = data.Level4AssignedToTypeId;
                                viewModel.AssignedToUserId = data.Level4AssignedToUserId;
                                viewModel.AssignedToTeamId = data.Level4AssignedToTeamId;
                                viewModel.AssignedToTypeCode = assType.Code;
                            }
                            break;

                        default: break;
                    }
                }
                             
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
