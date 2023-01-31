using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Interface.BusinessScript.Note.General.DocumentManagement
{
    public interface INoteGeneralDMSPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> ManageWorkspacePermission(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdateIsLatestRevision(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

        Task<CommandResult<NoteTemplateViewModel>> CreateWorkflowService(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

    }
}
