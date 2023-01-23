using CMS.Business.Interface.BusinessScript.Task.General.PerformanceManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Task.General.PerformanceManagement
{
    public class TaskGeneralPerformanceManagementPreScript : ITaskGeneralPerformanceManagementPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> FreezePerformanceDocumentTask(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var data = await _pmsBusiness.GetPerformanceDocumentMasterByServiceId(viewModel.ParentServiceId);
            if(data!=null)
            {
                if (data.DocumentStatus == PerformanceDocumentStatusEnum.Freezed)
                {
                    var errorList = new Dictionary<string, string>();
                    errorList.Add("Freeze", "The Performance Document Master is Freezed.");
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        /// <summary>
        /// Update ServiceOwnerUser as StepTaskAssignee
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateStepTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serBus = sp.GetService<IServiceBusiness>();
            var _taskBus = sp.GetService<ITaskBusiness>();
            var data = await _serBus.GetSingleById(viewModel.ParentServiceId);
            if (data != null)
            {
                //if (data.OwnerUserId.IsNullOrEmpty())
                //{
                //    var errorList = new Dictionary<string, string>();
                //    errorList.Add("Freeze", "The Performance Document Master is Freezed.");
                //    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                //}
                var result = await _taskBus.UpdateStepTaskAssignee(viewModel.TaskId,data.OwnerUserId);
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel,true,"");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
