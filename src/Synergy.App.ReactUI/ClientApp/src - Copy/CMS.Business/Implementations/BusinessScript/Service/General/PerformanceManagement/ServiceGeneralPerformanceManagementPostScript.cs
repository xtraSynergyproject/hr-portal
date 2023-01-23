
using CMS.Business.Interface.BusinessScript.Service.General.PerformanceManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.PerformanceManagement
{
    public class ServiceGeneralPerformanceManagementPostScript : IServiceGeneralPerformanceManagementPostScript
    {
        /// <summary>
        /// This method used trigger mid year and end year review task to employee and manager
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerReviewGoal(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {            
            var _performanceBusiness = sp.GetService<IPerformanceManagementBusiness>();
           // await _performanceBusiness.TriggerReviewGoal(viewModel);
           // await _performanceBusiness.TriggerReviewAdhocTasks(viewModel);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerAdhocTasksGoals(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode== "SERVICE_STATUS_COMPLETE")
            {
                var _performanceBusiness = sp.GetService<IPerformanceManagementBusiness>();
               // await _performanceBusiness.TriggerAdhocTasksGoals(viewModel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerAdhocTasksCompetency(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _performanceBusiness = sp.GetService<IPerformanceManagementBusiness>();
               // await _performanceBusiness.TriggerAdhocTasksCompetency(viewModel); 
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
