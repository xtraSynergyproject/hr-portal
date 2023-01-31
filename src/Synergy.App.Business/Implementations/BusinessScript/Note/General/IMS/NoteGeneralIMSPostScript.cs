using Synergy.App.Business.Interface.BusinessScript.Note.General.IMS;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.General.IMS
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
            var service1 = await _serviceBusiness.GetSingle(x=>x.UdfNoteTableId== RequisitionService);
            var service =await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = service1.Id });
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

        public async Task<CommandResult<NoteTemplateViewModel>> UpdateRequisitionItemPOQuantity(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _inventoryBusiness = sp.GetService<IInventoryManagementBusiness>();
            var req = await _inventoryBusiness.GetRequisitionItemById(Convert.ToString(udf.RequisitionItemId));
            var note = await _noteBusiness.GetSingleById(req.NoteId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.NoteId = note.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var POQuantity = rowData.ContainsKey("POQuantity") ? Convert.ToString(rowData["POQuantity"]) : "";
            rowData["POQuantity"] = (Convert.ToInt32(rowData["POQuantity"]) + Convert.ToInt32(udf.ItemQuantity)).ToString();
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> UpdatePOItemReceivedQuantity(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _inventoryBusiness = sp.GetService<IInventoryManagementBusiness>();
            if (udf.ReferenceHeaderItemId != null)
            {
                var req = await _inventoryBusiness.GetPOItemById(Convert.ToString(udf.ReferenceHeaderItemId));
                var note = await _noteBusiness.GetSingleById(req.NoteId);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = note.Id;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var POQuantity = rowData.ContainsKey("ReceivedQuantity") ? Convert.ToString(rowData["ReceivedQuantity"]) : "";
                rowData["ReceivedQuantity"] = (Convert.ToInt32(rowData["ReceivedQuantity"]) + Convert.ToInt32(udf.ItemQuantity)).ToString();
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                // update Closing Balance
                var itemId = Convert.ToString(udf.ItemId);
                var warehouseId = Convert.ToString(udf.WarehouseId);
                var closingBalance = await _inventoryBusiness.GetClosingBalance(itemId, warehouseId);
                await _inventoryBusiness.UpdateStockClosingBalance(itemId, warehouseId, closingBalance);
                
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }

}
