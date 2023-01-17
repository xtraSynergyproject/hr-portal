using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
using CMS.UI.ViewModel;

namespace CMS.Business.Interface.BusinessScript.Note.Galfar.DocumentManagement
{
    public interface IDocumentManagementPreScript
    {
        Task<CommandResult<NoteTemplateViewModel>> ValidateRequestForInspection(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> ValidateRequestForInspectionHalul(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<NoteTemplateViewModel>> ValidateDocumentRaisedServiceRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
