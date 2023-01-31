using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.BusinessScript.Task.General.DocumentManagement
{
    public interface ITaskGeneralDMSPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> OverwriteUdfs(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> CopySameNoteNoUdfs(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

    }
}
