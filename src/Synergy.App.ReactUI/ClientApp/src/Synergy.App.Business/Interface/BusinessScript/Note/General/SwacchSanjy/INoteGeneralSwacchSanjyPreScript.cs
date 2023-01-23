using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.SwacchSanjy
{
    public interface INoteGeneralSwacchSanjyPreScript
    {
        Task<CommandResult<NoteTemplateViewModel>> SaveUdfData(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
       
    }
}
