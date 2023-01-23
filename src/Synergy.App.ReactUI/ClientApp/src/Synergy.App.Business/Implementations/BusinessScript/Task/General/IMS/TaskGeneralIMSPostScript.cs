using Synergy.App.Business.Interface.BusinessScript.Task.General.IMS;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.IMS
{
    public class TaskGeneralIMSPostScript : ITaskGeneralIMSPostScript
    {

        /// <summary>
        /// ValidateApprovedRequisitionItems
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> CompleteServiceonTaskCompletion(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            if (viewModel.TaskStatusCode=="TASK_STATUS_COMPLETED") 
            {
                ServiceTemplateViewModel model = new ServiceTemplateViewModel();
                model.ServiceId = viewModel.ParentServiceId;
                model.DataAction = DataActionEnum.Edit;
                var serviceData = await _serviceBusiness.GetServiceDetails(model);
                serviceData.ServiceStatusCode = "SERVICE_STATUS_COMPLETED";
                var result=await _serviceBusiness.ManageService(serviceData);
               
            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
