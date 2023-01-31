using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.IMS
{
    public interface ITaskGeneralIMSPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> CompleteServiceonTaskCompletion(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
