using Synergy.App.Business.BusinessScript.Service.General.DocumentManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.DocumentManagement
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

        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerEPETask(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _tempBusiness = sp.GetService<ITemplateBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                string documentId = udf.documentId;
                string ServiceId = udf.ServiceId;
                var note = await _noteBusiness.GetSingleById(documentId);
                var noteTemplate = await _tempBusiness.GetSingleById(note.TemplateId);
                var taskTempModel = new TaskTemplateViewModel();
                taskTempModel.ActiveUserId = uc.UserId;
                taskTempModel.TemplateCode = "EPEReview";
                var task = await _taskBusiness.GetTaskDetails(taskTempModel);
                
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("documentId", documentId);
                if (noteTemplate.IsNotNull())
                {
                    ((IDictionary<String, Object>)exo).Add("noteTemplateCode", noteTemplate.Code);
                }                
                task.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                task.AssignedToUserId = uc.UserId;
                task.TaskSubject = "EPE Review and Comments";
                task.DataAction = DataActionEnum.Create;
                task.ParentServiceId = ServiceId;
                var result = await _taskBusiness.ManageTask(task);

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> AddReferenceId(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                string documentId = udf.documentId;
                string ServiceId = udf.ServiceId;
                var noteDetails = await _noteBusiness.GetSingleById(documentId);
                if (noteDetails.IsNotNull())
                {
                    noteDetails.ReferenceId = ServiceId;
                    noteDetails.DataAction = DataActionEnum.Edit;
                    var res = await _noteBusiness.Edit(noteDetails);
                    if (!res.IsSuccess)
                    {
                        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, $"Error in Adding ReferenceId to Document");
                    }
                }
                else
                {
                    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, $"Document does not Exist");
                }
               

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }




    }
}
