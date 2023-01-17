using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.DocumentManagement
{
    public interface INoteGeneralDMSPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> ManageWorkspacePermission(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdateIsLatestRevision(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

        Task<CommandResult<NoteTemplateViewModel>> CreateWorkflowService(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> RemoveReferenceId(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

    }
}
