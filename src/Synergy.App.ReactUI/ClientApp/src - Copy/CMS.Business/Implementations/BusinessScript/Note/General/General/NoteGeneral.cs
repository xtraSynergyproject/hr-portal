using CMS.Business.BusinessScript.Note.General.General;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Note.Galfar.General.General
{
    public class NoteGeneral : INoteGeneral
    {
        public CommandResult<NoteTemplateViewModel> Test(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
    }
}
