using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.PerformanceManagement
{
    public interface INoteGeneralPMSPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> MapDepartmentUser(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
       

    }
}
