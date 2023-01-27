using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.BLS
{
    public interface ITaskGeneralBLSPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> TriggerEmailOnBLSVisaAppointment(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> TriggerEmailOnBLSVisaStamping(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);


    }
}
