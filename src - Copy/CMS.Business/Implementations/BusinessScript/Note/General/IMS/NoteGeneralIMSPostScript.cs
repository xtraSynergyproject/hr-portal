using CMS.Business.Interface.BusinessScript.Note.General.IMS;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Note.General.IMS
{
    public class NoteGeneralIMSPostScript : INoteGeneralIMSPostScript
    {
        public async Task<CommandResult<NoteTemplateViewModel>> ManageRequisitionValue(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var note = await _noteBusiness.GetSingleById(viewModel.NoteId);
            var noteTempModel = new NoteTemplateViewModel();
            //noteTempModel.TemplateCode = note.TemplateCode;
            noteTempModel.NoteId = note.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);            
            var RequisitionService = rowData1.ContainsKey("RequisitionId") ? Convert.ToString(rowData1["RequisitionId"]) : "";
            var Amount = rowData1.ContainsKey("Amount") ? Convert.ToString(rowData1["Amount"]) : "";
           
            // GET Service 
            var service =await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = RequisitionService });
            var noteTempModel1 = new NoteTemplateViewModel();            
            noteTempModel1.SetUdfValue = true;
            noteTempModel1.NoteId = service.UdfNoteId;
            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["RequisitionValue"] = (Convert.ToDecimal(rowData["RequisitionValue"]) +Convert.ToDecimal(Amount)).ToString();
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }       
    }

}
