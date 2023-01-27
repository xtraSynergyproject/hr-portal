using CMS.Business.BusinessScript.Service.General.DocumentManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.DocumentManagement
{
    public class ServiceGeneralDocumentManagement : IServiceGeneralDocumentManagement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateProjectDocumentPendingStatus(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var documentId = Convert.ToString(rowData["documentId"]);
            var noteDetails = await _noteBusiness.GetSingleById(documentId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.TemplateCode = noteDetails.TemplateCode;
            noteTempModel.NoteId = noteDetails.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var stageStatus = await _lovBusiness.GetSingle(x=>x.Code== "InternalReview_Status");
            if (stageStatus.IsNotNull())
            {
                rowData1["stageStatus"] =stageStatus.Id;
            }
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
            var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
            
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> CopyAttachment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var documentId = Convert.ToString(rowData["documentId"]);
            var noteDetails = await _noteBusiness.GetSingleById(documentId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.TemplateCode = noteDetails.TemplateCode;
            noteTempModel.NoteId = noteDetails.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var attachmentValue = rowData1.ContainsKey("attachment") ? rowData1["attachment"].ToString() : rowData1.ContainsKey("fileAttachment") ? rowData1["fileAttachment"].ToString() : "";

            var noteUdf = await _noteBusiness.GetSingleById(viewModel.UdfNoteId);
            var noteUdfTempModel = new NoteTemplateViewModel();
            noteUdfTempModel.TemplateCode = noteUdf.TemplateCode;
            noteUdfTempModel.NoteId = noteUdf.Id;
            noteUdfTempModel.SetUdfValue = true;
            var noteUdfmodel = await _noteBusiness.GetNoteDetails(noteUdfTempModel);
            var rowDataUdf = noteUdfmodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            if (attachmentValue.IsNotNull())
            {
                rowDataUdf["File"] = attachmentValue;
            }

            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowDataUdf);
            var update = await _noteBusiness.EditNoteUdfTable(noteUdfmodel, data1, noteUdfmodel.UdfNoteTableId);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateVendorInspectionDocumentPendingStatus(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var documentId = Convert.ToString(rowData["documentId"]);
            var noteDetails = await _noteBusiness.GetSingleById(documentId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.TemplateCode = noteDetails.TemplateCode;
            noteTempModel.NoteId = noteDetails.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var stageStatus = await _lovBusiness.GetSingle(x => x.Code == "GAL_STATUS");
            if (stageStatus.IsNotNull())
            {
                rowData1["stageStatus"] = stageStatus.Id;
            }
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
            var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


    }
}
