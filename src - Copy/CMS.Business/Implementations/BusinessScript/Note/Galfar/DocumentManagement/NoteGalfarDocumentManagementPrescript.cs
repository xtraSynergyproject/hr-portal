using CMS.Business.Interface.BusinessScript.Note.Galfar.DocumentManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Business.Interface.DMS;
namespace CMS.Business.Implementations.BusinessScript.Note.Galfar.DocumentManagement
{
   public class NoteGalfarDocumentManagementPrescript: IDocumentManagementPreScript
    {
        public async Task<CommandResult<NoteTemplateViewModel>> ValidateRequestForInspection(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var errorList = new Dictionary<string, string>();
            var _business = sp.GetService<IDMSDocumentBusiness>();
            var res = await _business.ValidateRequestForInspection(viewModel, udf, errorList);
            if (res == false)
            {

                //errorList.Add("Validate", "This User is already mapped to a different Person,Please Select different User");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<NoteTemplateViewModel>> ValidateRequestForInspectionHalul(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var errorList = new Dictionary<string, string>();
            var _business = sp.GetService<IDMSDocumentBusiness>();
            var res = await _business.ValidateRequestForInspectionHalul(viewModel, udf, errorList);
            if (res == false)
            {

                //errorList.Add("Validate", "This User is already mapped to a different Person,Please Select different User");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// ValidateDocumentRaisedServiceRequest : Validate for Document Have any Raised service request in InProgress
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> ValidateDocumentRaisedServiceRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var errorList = new Dictionary<string, string>();
            var _business = sp.GetService<IDMSDocumentBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _templatebusiness = sp.GetService<ITemplateBusiness>();
            var template = await _templatebusiness.GetSingleById(viewModel.TemplateId);
            var res = true;
            if (template != null)
            {
                var data = await _business.GetWorflowDetailByDocument(viewModel.NoteId);
                if (data.IsNotNull())
                {
                    if (data.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
                    {
                        res = false;
                    }
                }
            }
            // call business method
            if (res == false)
            {
                errorList.Add("Validate", "This Document already have raised service with InProgress status, Edit document not allowed. ");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }
}
