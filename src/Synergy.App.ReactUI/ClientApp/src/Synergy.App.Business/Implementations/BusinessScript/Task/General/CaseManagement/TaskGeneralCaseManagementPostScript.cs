using Synergy.App.Business.Interface.BusinessScript.Task.General.CaseManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.CaseManagement
{
    public class TaskGeneralCaseManagementPostScript : ITaskGeneralCaseManagementPostScript
    {
        /// <summary>
        /// AddTagging - it will tag task to relevent goal and compentency
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> CompleteServiceonTaskComplete(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var code = await _lovBusiness.GetSingleById(udf.ActionTakenId);
                if (code.Code != "FORWARD_VENDOR")
                {                  
                    ServiceTemplateViewModel model = new ServiceTemplateViewModel();
                    model.ServiceId = viewModel.ParentServiceId;
                    model.DataAction = DataActionEnum.Edit;
                    var serviceData = await _serviceBusiness.GetServiceDetails(model);
                    serviceData.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                    var result = await _serviceBusiness.ManageService(serviceData);
                }

            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        
    }
}
