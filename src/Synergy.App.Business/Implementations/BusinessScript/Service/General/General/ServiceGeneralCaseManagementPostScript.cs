
using Synergy.App.Business.Interface.BusinessScript.Service.General.General;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.General
{
    public class ServiceGeneralCaseManagementPostScript : IServiceGeneralCaseManagementPostScript
    {
        /// <summary>
        /// Trigger External Service step task - it will trigger service upon service completion
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
      
      
        public async Task<CommandResult<ServiceTemplateViewModel>> InitiateExternalServiceStepTask(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _componentResultBusiness = sp.GetService<IComponentResultBusiness>();
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_SERVICE_STATUS");
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                string externalServiceId = udf.externalServiceRequest;// await _serviceBusiness.GetSingleById(udf.externalServiceRequest);
               var internalservices=await _serviceBusiness.GetInternalServiceListFromExternalRequestId(externalServiceId);
                if (internalservices!=null && internalservices.Count()>0) 
                {
                    // Check other internal services are completed or not
                    if (!internalservices.Any(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"))
                    {
                        // Do not Complete the second task as internal services are not yet completed
                    }
                    else 
                    {
                        // Complete step task as the internal services are already triggered
                        var externalServiceStepTask =await _componentResultBusiness.GetStepTaskList(externalServiceId);
                        var secondStepTask = externalServiceStepTask.Where(x => x.TemplateMasterCode == "AW_INT_RES").FirstOrDefault();
                        if (secondStepTask!=null) 
                        {
                            var task = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel()
                            {
                                TaskId = secondStepTask.Id,
                                DataAction = DataActionEnum.Edit
                            }); 
                            task.TaskStatusCode = "TASK_STATUS_COMPLETE";
                            task.CompletedDate = DateTime.Now;
                            task.CompleteReason = "Task is completed as all the internal services are completed";
                            await _taskBusiness.ManageTask(task);
                        }

                    }
                }
                
               

            }
              
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        
    }
}
