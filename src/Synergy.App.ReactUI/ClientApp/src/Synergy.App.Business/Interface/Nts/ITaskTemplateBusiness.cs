using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITaskTemplateBusiness : IBusinessBase<TaskTemplateViewModel,TaskTemplate>
    {
        Task<CommandResult<TaskTemplateViewModel>> ManageCreate(TaskTemplateViewModel model, bool autoCommit = true);
        Task<CommandResult<TaskTemplateViewModel>> ManageEdit(TaskTemplateViewModel model, bool autoCommit = true);

        Task<CommandResult<TaskTemplateViewModel>> CopyTaskTemplate(TaskTemplateViewModel oldModel, string newTempId, CopyTemplateViewModel copyModel = null);
    }
}
