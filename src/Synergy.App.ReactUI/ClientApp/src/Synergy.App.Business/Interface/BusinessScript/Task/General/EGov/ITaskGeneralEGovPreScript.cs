using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.EGov
{
    public interface ITaskGeneralEGovPreScript
    {
        Task<CommandResult<TaskTemplateViewModel>> ChangeFieldInspectorAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> ChangeApproverAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> ChangeBillCollectorAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> ChangeRevenueOfficerAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> ChangePaymentGatewayAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> ChangeAssigneeToServiceOwner(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeToServiceOwner(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeToConsumer(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeForDeliverBinRequest(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> JSCChangeAssigneeForCompleteGarbageCollection(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> SetAssigeeDetailsForJammuGrievance(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
