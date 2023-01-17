using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Interface.BusinessScript.Task.General.EGov
{
    public interface ITaskGeneralEGovPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> UpdateRentalRenewalDate(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> UpdateRentalStatus(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
