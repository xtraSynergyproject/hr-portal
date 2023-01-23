using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Service.Galfar.Recruitment
{
    public interface IServiceGalfarRecruitmentPostScript
    {
        Task<CommandResult<ServiceTemplateViewModel>> TriggerIntentToOffer(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerWorkerAppointment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerFinalOffer(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggeVisa(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerTravelling(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerJoining(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> ScheduleForInterview(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerCRPFAppointment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> PublishToCareerPortal(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> CreatePersonPositionandAssignment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
