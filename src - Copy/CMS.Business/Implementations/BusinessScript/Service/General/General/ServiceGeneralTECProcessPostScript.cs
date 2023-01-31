
using CMS.Business.Interface.BusinessScript.Service.General.General;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.General
{
    public class ServiceGeneralTECProcessPostScript : IServiceGeneralTECProcessPostScript
    {
        /// <summary>
        /// Trigger External Service step task - it will trigger service upon service completion
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
      
      
        public async Task<CommandResult<ServiceTemplateViewModel>> CreateChildServices(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();           
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var temlatesCode = new String[] { "CHANGE_PASSWORD", "IT_RESOURCE_COLLECTION", "SEATING_ARRANGEMENT", "ACCESS_CARD_DEACTIVATION", "PARKING_CARD_DEACTIVATION", "GR/CC_OPERATION", "FINAL_SETTELMENT", "EMAIL_REMOVAL" };
                for (int i = 0; i < temlatesCode.Length; i++)
                {
                    var serviceTemplate = new ServiceTemplateViewModel();
                    serviceTemplate.ActiveUserId = viewModel.ActiveUserId;
                    serviceTemplate.TemplateCode = temlatesCode[i];
                    var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);

                    service.ServiceSubject = service.TemplateDisplayName;
                    service.OwnerUserId = viewModel.ActiveUserId;
                    service.StartDate = DateTime.Now;
                    service.DueDate = DateTime.Now.AddDays(10);
                    service.DataAction = DataActionEnum.Create;
                    service.ParentServiceId = viewModel.ServiceId;
                    service.ServicePlusId = viewModel.ServiceId;
                    if (i == 0)
                    {
                        service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    }
                    else
                    {
                        service.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    }
                    Hangfire.BackgroundJob.Enqueue<HangfireScheduler>(x => x.CreateService(service));
                    //var res = await _serviceBusiness.ManageService(service);
                }
            }
              
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerNextServices(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {                       
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                switch (viewModel.TemplateCode)
                {
                    case "CHANGE_PASSWORD":                        
                        await CreateServices(viewModel, udf, uc, sp, new String[] { "IT_RESOURCE_COLLECTION" });
                        break;
                    case "IT_RESOURCE_COLLECTION":
                        await CreateServices(viewModel, udf, uc, sp, new String[] {  "SEATING_ARRANGEMENT" });
                        break;
                    case "SEATING_ARRANGEMENT":
                        await CreateServices(viewModel, udf, uc, sp, new String[] {  "ACCESS_CARD_DEACTIVATION" });
                        break;
                    case "ACCESS_CARD_DEACTIVATION":

                        await CreateServices(viewModel, udf, uc, sp, new String[] {  "PARKING_CARD_DEACTIVATION" });
                        break;
                    case "PARKING_CARD_DEACTIVATION":
                        await CreateServices(viewModel, udf, uc, sp, new String[] {  "GR/CC_OPERATION" });

                        break;
                    case "GR/CC_OPERATION":
                        await CreateServices(viewModel, udf, uc, sp, new String[] {  "FINAL_SETTELMENT" });

                        break;
                    case "FINAL_SETTELMENT":
                        await CreateServices(viewModel, udf, uc, sp, new String[] {  "EMAIL_REMOVAL" });
                        break;
                    case "EMAIL_REMOVAL":
                        await CompleteTask(viewModel, udf, uc, sp, new String[] { "CloseOffBoardingProcess" });
                        break;                    
                    default:
                        break;
                }
               
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> CreateServices(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp,string[] temlatesCode)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();            
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {                
                for (int i = 0; i < temlatesCode.Length; i++)
                {
                    var serviceTemplate = new ServiceTemplateViewModel();
                    serviceTemplate.ActiveUserId = viewModel.ActiveUserId;                    
                    var serModel = await _serviceBusiness.GetSingle(x => x.ParentServiceId == viewModel.ParentServiceId && x.ServicePlusId== viewModel.ParentServiceId && x.TemplateCode == temlatesCode[i]);
                    serviceTemplate.ServiceId = serModel.Id;
                    var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);

                    service.ServiceSubject = service.TemplateDisplayName;                    
                    service.StartDate = DateTime.Now;
                    service.DueDate = DateTime.Now.AddDays(10);
                    service.DataAction = DataActionEnum.Edit;                                      
                    service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    service.ServicePlusId = viewModel.ParentServiceId;
                    service.ParentServiceId = viewModel.ParentServiceId;
                    Hangfire.BackgroundJob.Enqueue<HangfireScheduler>(x => x.CreateService(service));
                    //var res = await _serviceBusiness.ManageService(service);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> CompleteTask(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp, string[] temlatesCode)
        {                       
            var _taskBusiness = sp.GetService<ITaskBusiness>();            
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                for (int i = 0; i < temlatesCode.Length; i++)
                {
                    var taskTemplate = new TaskTemplateViewModel();
                    taskTemplate.ActiveUserId = viewModel.ActiveUserId;                    
                    var taskModel = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ParentServiceId && x.TemplateCode == temlatesCode[i]);
                    taskTemplate.TaskId = taskModel.Id;
                    taskTemplate.TaskTemplateType = TaskTypeEnum.StepTask;
                    var task = await _taskBusiness.GetTaskDetails(taskTemplate);

                    task.TaskSubject = task.TemplateDisplayName;
                    task.OwnerUserId = viewModel.ActiveUserId;
                    task.StartDate = DateTime.Now;
                    task.DueDate = DateTime.Now.AddDays(10);
                    task.DataAction = DataActionEnum.Edit;
                    task.TaskStatusCode = "TASK_STATUS_COMPLETE";                    
                    task.ParentServiceId = viewModel.ParentServiceId;                    
                    var res = await _taskBusiness.ManageTask(task);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
