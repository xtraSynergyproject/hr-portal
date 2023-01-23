using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Note.General.CoreHR
{
    public interface INoteGeneralCoreHRPostScript
    {
        Task<CommandResult<NoteTemplateViewModel>> CreatePositionHierarchy(NoteTemplateViewModel viewModel, dynamic udf,IUserContext uc,IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreateDepartmentHierarchy(NoteTemplateViewModel viewModel, dynamic udf,IUserContext uc,IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdatePositionNameOnJobChange(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdatePositionNameOnDepartmentChange(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreateEmployeeBook(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> EmployeeBookPostScript(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreateHybridHierarchy(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> TriggerNewPersonRequestService(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdateDepartmentNameRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdatePersonJob(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdateJobNameOnRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdatePersonDepartment(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> UpdateEmployeeDetailOnTerminate(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreatePositionOnNewPositionRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreatePersonOnNewPerson(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreateJobOnNewJob(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreateCareerLevelOnNewCareerLevel(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> CreateDepartmentOnNewDepartment(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
