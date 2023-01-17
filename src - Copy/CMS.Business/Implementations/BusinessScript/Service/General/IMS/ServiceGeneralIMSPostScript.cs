using CMS.Business.Interface.BusinessScript.Service.General.IMS;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.IMS
{
    public class ServiceGeneralIMSPostScript : IServiceGeneralIMSPostScript
    {   
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateIssuedQuantityInRequisitionItem(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                // Get All Issued Items
                var items=await _inventoryManagementBusiness.GetRequisistionIssueItems(viewModel.ServiceId);
                if (items.IsNotNull() && items.Count>0) 
                {
                    foreach (var item in items) 
                    {                       
                        var noteTempModel = new NoteTemplateViewModel();                       
                        noteTempModel.NoteId = item.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var CurrentIssueQuantity = rowData1.ContainsKey("IssuedQuantity") ? Convert.ToString(rowData1["IssuedQuantity"]) : "0";
                        var RequisitionItemId = rowData1.ContainsKey("RequisitionItemId") ? Convert.ToString(rowData1["RequisitionItemId"]) : "";

                        // GET requisition Item            
                        var data = await _tableMetadataBusiness.GetTableDataByColumn("IMS_REQUSITION_ITEM", "", "Id", RequisitionItemId);
                        if (data != null)
                        {
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.SetUdfValue = true;
                            noteTempModel1.NoteId = data["NtsNoteId"].ToString();
                            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            rowData["IssuedQuantity"] = rowData.ContainsKey("IssuedQuantity") ? (Convert.ToDecimal(rowData["IssuedQuantity"]) + Convert.ToDecimal(CurrentIssueQuantity)).ToString() : (0 + Convert.ToDecimal(CurrentIssueQuantity));
                            if (Convert.ToDecimal(rowData["IssuedQuantity"]) >= Convert.ToDecimal(rowData["ApprovedQuantity"]))
                            {
                                rowData["Issued"] = "True";
                            }
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                            var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                        }
                    }
                  
                }
                var serviceTempModel1 = new NoteTemplateViewModel();
                serviceTempModel1.SetUdfValue = true;
                serviceTempModel1.NoteId = viewModel.UdfNoteId;
                var servicemodel = await _noteBusiness.GetNoteDetails(serviceTempModel1);
                var rowData2 = servicemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var requisitionId = rowData2.ContainsKey("RequisitionId")? rowData2["RequisitionId"].ToString():null;
                var requisitionitems = await _inventoryManagementBusiness.GetRequisistionItemsData(requisitionId);
                if (requisitionitems != null && requisitionitems.Count > 0 && !requisitionitems.Any(x => x.Issued == false))
                {
                    // upadte Requisition Service Issued To true if all items are issued
                    await _inventoryManagementBusiness.UpdateRequisitionServiceToIssued(requisitionId);
                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }

}
