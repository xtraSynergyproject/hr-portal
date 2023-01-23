using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.BusinessScript.Note.General.General
{
    public interface INoteGeneral
    {
        CommandResult<NoteTemplateViewModel> Test(NoteTemplateViewModel viewModel, dynamic udf,IUserContext uc,IServiceProvider sp);
    }
}
