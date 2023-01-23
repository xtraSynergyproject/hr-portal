using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.BusinessScript.Service.General.DocumentManagement
{
    public interface IServiceGeneralDocumentManagement
    {
        CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> UpdateProjectDocumentPendingStatus(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> CopyAttachment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerEPETask(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> AddReferenceId(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        
    }
}
