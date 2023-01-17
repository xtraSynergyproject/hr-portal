using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface ITaskTemplateBusiness : IBusinessBase<TaskTemplateViewModel,TaskTemplate>
    {
        Task<CommandResult<TaskTemplateViewModel>> ManageCreate(TaskTemplateViewModel model);
        Task<CommandResult<TaskTemplateViewModel>> ManageEdit(TaskTemplateViewModel model);
    }
}
