using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.SwacchSanjy
{
    public interface ITaskGeneralSwacchSanjyPreScript
    {       
        Task<CommandResult<TaskTemplateViewModel>> ChangeVerifyIssueTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
