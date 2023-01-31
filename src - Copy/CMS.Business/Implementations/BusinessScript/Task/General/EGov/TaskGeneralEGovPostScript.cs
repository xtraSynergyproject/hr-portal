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
    }
}
