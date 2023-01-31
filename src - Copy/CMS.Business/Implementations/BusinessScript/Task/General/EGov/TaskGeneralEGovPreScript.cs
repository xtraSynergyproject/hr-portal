using CMS.Business.Interface.BusinessScript.Task.General.EGov;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Task.General.EGov
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
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
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
    }
}
