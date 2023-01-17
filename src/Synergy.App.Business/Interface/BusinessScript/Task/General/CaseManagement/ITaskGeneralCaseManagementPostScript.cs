using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.CaseManagement
{
    public interface ITaskGeneralCaseManagementPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> CompleteServiceonTaskComplete(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
