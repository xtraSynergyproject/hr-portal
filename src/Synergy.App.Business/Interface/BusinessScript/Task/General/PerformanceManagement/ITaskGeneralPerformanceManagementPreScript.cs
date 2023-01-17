using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.PerformanceManagement
{
    public interface ITaskGeneralPerformanceManagementPreScript
    {
        Task<CommandResult<TaskTemplateViewModel>> FreezePerformanceDocumentTask(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> UpdateStepTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> CheckWeightageOfEmployeeReview(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> CheckWeightageOfManagerReview(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
