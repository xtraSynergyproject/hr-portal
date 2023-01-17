using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Interface.BusinessScript.Task.General.PerformanceManagement
{
    public interface ITaskGeneralPerformanceManagementPreScript
    {
        Task<CommandResult<TaskTemplateViewModel>> FreezePerformanceDocumentTask(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> UpdateStepTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
