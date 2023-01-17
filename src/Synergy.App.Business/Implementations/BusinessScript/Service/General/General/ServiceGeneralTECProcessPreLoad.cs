
using Hangfire;
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
    public class ServiceGeneralTECProcessPreLoad : IServiceGeneralTECProcessPreLoad
    {
        //private readonly IHangfireScheduler _hangfireScheduler;
        public ServiceGeneralTECProcessPreLoad(
            //IHangfireScheduler hangfireScheduler
            )
        {
           // _hangfireScheduler = hangfireScheduler;
        }

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
                    var hangfireScheduler = sp.GetService<IHangfireScheduler>();
                    await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.CreateService(service));
                    //var res = await _serviceBusiness.ManageService(service);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
