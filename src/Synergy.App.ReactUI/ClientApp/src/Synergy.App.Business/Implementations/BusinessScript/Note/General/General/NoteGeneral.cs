using Synergy.App.Business.BusinessScript.Note.General.General;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.Galfar.General.General
{
    public class NoteGeneral : INoteGeneral
    {
        public CommandResult<NoteTemplateViewModel> Test(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
    }
}
