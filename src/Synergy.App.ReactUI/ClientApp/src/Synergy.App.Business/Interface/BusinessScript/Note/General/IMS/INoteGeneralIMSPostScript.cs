using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.IMS
{
    public interface INoteGeneralIMSPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> ManageRequisitionValue(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdateRequisitionItemPOQuantity(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdatePOItemReceivedQuantity(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
