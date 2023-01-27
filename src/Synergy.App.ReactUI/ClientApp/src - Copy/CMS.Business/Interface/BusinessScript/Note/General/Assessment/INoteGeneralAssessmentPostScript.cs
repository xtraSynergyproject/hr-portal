using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Interface.BusinessScript.Note.General.Assessment
{
    public interface INoteGeneralAssessmentPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> ManageSurveyUsers(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);       
    }
}
