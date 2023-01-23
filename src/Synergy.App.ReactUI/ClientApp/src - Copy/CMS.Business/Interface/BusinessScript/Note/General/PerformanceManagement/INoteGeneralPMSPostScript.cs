using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Interface.BusinessScript.Note.General.PerformanceManagement
{
    public interface INoteGeneralPMSPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> MapDepartmentUser(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
       

    }
}
