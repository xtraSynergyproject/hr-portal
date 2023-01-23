using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.CoreHR
{
    public interface INoteGeneralCoreHRPreScript
    {
        Task<CommandResult<NoteTemplateViewModel>> ValidatePersonUser(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> ValidateCalendarHoliday(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> ValidateCalendar(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> ValidateUniqueDepartment(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> ValidateEmployeeBook(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
