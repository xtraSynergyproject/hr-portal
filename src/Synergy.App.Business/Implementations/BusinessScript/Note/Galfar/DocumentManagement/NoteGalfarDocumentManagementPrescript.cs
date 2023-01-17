using Synergy.App.Business.Interface.BusinessScript.Note.Galfar.DocumentManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Business.Interface.DMS;
namespace Synergy.App.Business.Implementations.BusinessScript.Note.Galfar.DocumentManagement
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
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _templatebusiness = sp.GetService<ITemplateBusiness>();
            var _lovbusiness = sp.GetService<ILOVBusiness>();
            var template = await _templatebusiness.GetSingleById(viewModel.TemplateId);
            var res = true;
            if (template != null)
            {
                //var data = await _business.GetWorflowDetailByDocument(viewModel.NoteId);
                var notedata = await _noteBusiness.GetSingleById(viewModel.NoteId);
                if (notedata.IsNotNull() && notedata.ReferenceId.IsNotNullAndNotEmpty())
                {
                    var serviceData = await _serviceBusiness.GetSingleById(notedata.ReferenceId);
                    if (serviceData.IsNotNull())
                    {
                        var status = await _lovbusiness.GetSingleById(serviceData.ServiceStatusId);
                        if (status.Code == "SERVICE_STATUS_INPROGRESS" || status.Code== "SERVICE_STATUS_OVERDUE")
                        {
                            res = false;
                        }

                    }
                    //if (data.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
                    //{
                    //    res = false;
                    //}
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
