using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.IIP
{
    public interface INoteGeneralIip
    {
        Task<CommandResult<NoteTemplateViewModel>> TriggerHangfireForApiDataMigration(NoteTemplateViewModel viewModel, dynamic udf,IUserContext uc,IServiceProvider sp);
    }
}
