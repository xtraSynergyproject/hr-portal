using Synergy.App.Business.Interface.BusinessScript.Task.General.EGov;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.EGov
{
    public class TaskGeneralEGovPostScript : ITaskGeneralEGovPostScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
              
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateRentalRenewalDate(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
                        
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            { 
                    await _egovBusiness.UpdateRenewalEndDate(udf);
            }
           
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> UpdateRentalStatus(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
            
            await _egovBusiness.UpdateRentalStatus(viewModel,udf);            

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> UpdateProjectProposalStatus(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE" || viewModel.TaskStatusCode == "TASK_STATUS_REJECT")
            {

                await _egovBusiness.UpdateProjectProposalStatus(udf, viewModel.TaskStatusCode);
            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> UpdateJSCPaymentDetails(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _smcBusiness = sp.GetService<ISmartCityBusiness>();
            if (viewModel.TaskStatusCode == "TASK_STATUS_INPROGRESS")
            {
                await _smcBusiness.UpdatePaymentDetails(udf, viewModel);
            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
