using Synergy.App.Business.Interface.BusinessScript.Service.General.IMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.IMS
{
    public class ServiceGeneralIMSPostScript : IServiceGeneralIMSPostScript
    {   
        //public async Task<CommandResult<ServiceTemplateViewModel>> UpdateIssuedQuantityInRequisitionItem(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        //{
        //    var _noteBusiness = sp.GetService<INoteBusiness>();
        //    //var _serviceBusiness = sp.GetService<IServiceBusiness>();
        //    var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
        //    var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
        //    var _lovBusiness = sp.GetService<ILOVBusiness>();
        //    if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
        //    {
        //        // Get All Issued Items
        //        var items=await _inventoryManagementBusiness.GetRequisistionIssueItems(viewModel.ServiceId);
        //        if (items.IsNotNull() && items.Count>0) 
        //        {
        //            foreach (var item in items) 
        //            {                       
        //                var noteTempModel = new NoteTemplateViewModel();                       
        //                noteTempModel.NoteId = item.NoteId;
        //                noteTempModel.SetUdfValue = true;
        //                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
        //                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
        //                var CurrentIssueQuantity = rowData1.ContainsKey("IssuedQuantity") ? Convert.ToString(rowData1["IssuedQuantity"]) : "0";
        //                var RequisitionItemId = rowData1.ContainsKey("RequisitionItemId") ? Convert.ToString(rowData1["RequisitionItemId"]) : "";

        //                // GET requisition Item            
        //                var data = await _tableMetadataBusiness.GetTableDataByColumn("IMS_REQUSITION_ITEM", "", "Id", RequisitionItemId);
        //                if (data != null)
        //                {
        //                    var noteTempModel1 = new NoteTemplateViewModel();
        //                    noteTempModel1.SetUdfValue = true;
        //                    noteTempModel1.NoteId = data["NtsNoteId"].ToString();
        //                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
        //                    var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
        //                    rowData["IssuedQuantity"] = rowData.ContainsKey("IssuedQuantity") ? (Convert.ToDecimal(rowData["IssuedQuantity"]) + Convert.ToDecimal(CurrentIssueQuantity)).ToString() : (0 + Convert.ToDecimal(CurrentIssueQuantity));
        //                    if (Convert.ToDecimal(rowData["IssuedQuantity"]) >= Convert.ToDecimal(rowData["ApprovedQuantity"]))
        //                    {
        //                        rowData["Issued"] = "True";
        //                    }
        //                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
        //                    var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
        //                }
        //            }
                  
        //        }
        //        var serviceTempModel1 = new NoteTemplateViewModel();
        //        serviceTempModel1.SetUdfValue = true;
        //        serviceTempModel1.NoteId = viewModel.UdfNoteId;
        //        var servicemodel = await _noteBusiness.GetNoteDetails(serviceTempModel1);
        //        var rowData2 = servicemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
        //        var requisitionId = rowData2.ContainsKey("RequisitionId")? rowData2["RequisitionId"].ToString():null;
        //        var requisitionitems = await _inventoryManagementBusiness.GetRequisistionItemsData(requisitionId);
        //        if (requisitionitems != null && requisitionitems.Count > 0 && !requisitionitems.Any(x => x.Issued == false))
        //        {
        //            // upadte Requisition Service Issued To true if all items are issued
        //            await _inventoryManagementBusiness.UpdateRequisitionServiceToIssued(requisitionId);
        //        }
        //    }
        //    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        //}


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateIssuedQuantityInRequisitionItem(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            // Update it in progress
            //if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                // Get All Issued Items
                var reftype =Convert.ToString(udf.IssueReferenceType);
                var issueReferenceType = (ImsIssueTypeEnum)Enum.Parse(typeof(ImsIssueTypeEnum), reftype, true);// (ImsIssueTypeEnum)Enum.ToObject(typeof(ImsIssueTypeEnum), reftype); 
                var items = await _inventoryManagementBusiness.GetRequisistionIssueItems(viewModel.UdfNoteTableId, issueReferenceType);
                if (items.IsNotNull() && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = item.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var CurrentIssueQuantity = rowData1.ContainsKey("IssuedQuantity") ? Convert.ToString(rowData1["IssuedQuantity"]) : "0";
                        var RequisitionItemId = rowData1.ContainsKey("ReferenceHeaderItemId") ? Convert.ToString(rowData1["ReferenceHeaderItemId"]) : "";
                        var IssuedFromStockIds = rowData1.ContainsKey("IssuedFromStockIds") ? Convert.ToString(rowData1["IssuedFromStockIds"]) : null;
                        
                    

                        if (IssuedFromStockIds.IsNotNullAndNotEmpty())
                        {
                            var sourceIds = IssuedFromStockIds.Split(",");
                            //string[] sourceIds = Array.ConvertAll((object[])IssuedFromStockIds, Convert.ToString);
                            foreach (var ids in sourceIds)
                            {
                                // GET good receipt Item            
                                var data = await _tableMetadataBusiness.GetTableDataByColumn("IMS_GOODS_RECEIPT_ITEM", "", "Id", ids);
                                if (data != null)
                                {
                                    var noteTempModel1 = new NoteTemplateViewModel();
                                    noteTempModel1.SetUdfValue = true;
                                    noteTempModel1.NoteId = data["NtsNoteId"].ToString();
                                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                                    var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                                    rowData["IssuedQuantity"] = rowData.ContainsKey("IssuedQuantity") && (rowData["IssuedQuantity"].ToString() != null && rowData["IssuedQuantity"].ToString() != "null" && rowData["IssuedQuantity"].ToString() != "") ? (Convert.ToDecimal(rowData["IssuedQuantity"]) + Convert.ToDecimal(CurrentIssueQuantity)).ToString() : (0 + Convert.ToDecimal(CurrentIssueQuantity));                                  
                                    rowData["BalanceQuantity"] = (Convert.ToDecimal(rowData["ItemQuantity"]) - Convert.ToDecimal(rowData["IssuedQuantity"])).ToString();
                                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                                    var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                                }
                                else 
                                {
                                    // check for item stock
                                    var stockdata = await _tableMetadataBusiness.GetTableDataByColumn("ITEM_STOCK", "", "Id", ids);
                                    if (stockdata != null)
                                    {
                                        var noteTempModel1 = new NoteTemplateViewModel();
                                        noteTempModel1.SetUdfValue = true;
                                        noteTempModel1.NoteId = stockdata["NtsNoteId"].ToString();
                                        var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                                        var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                                        rowData["IssuedQuantity"] = rowData.ContainsKey("IssuedQuantity") && (rowData["IssuedQuantity"].ToString() != null && rowData["IssuedQuantity"].ToString() != "null" && rowData["IssuedQuantity"].ToString() != "") ? (Convert.ToInt32(rowData["IssuedQuantity"]) + Convert.ToInt32(CurrentIssueQuantity)).ToString() : (0 + Convert.ToInt32(CurrentIssueQuantity));
                                        rowData["BalanceQuantity"] = (Convert.ToDecimal(rowData["OpeningQuantity"]) - Convert.ToDecimal(rowData["IssuedQuantity"])).ToString();
                                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                                        var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                                    }
                                }
                            }
                        }
                        System.Data.DataRow reqdata= null;
                        if (issueReferenceType == ImsIssueTypeEnum.Requisition) 
                        {
                            // update RequisitionItem issued Quantity
                             reqdata = await _tableMetadataBusiness.GetTableDataByColumn("IMS_REQUSITION_ITEM", "", "Id", RequisitionItemId);                           
                        }
                        else if (issueReferenceType == ImsIssueTypeEnum.StockTransfer)
                        {
                            // update Stock Item issued Quantity
                            reqdata = await _tableMetadataBusiness.GetTableDataByColumn("IMS_STOCK_TRANSFER_ITEM", "", "Id", RequisitionItemId);                           
                        }
                        else if (issueReferenceType == ImsIssueTypeEnum.StockAdjustment)
                        {
                            // update Stock Adjustment Item issued Quantity
                            reqdata = await _tableMetadataBusiness.GetTableDataByColumn("STOCK_ADJUSTMENT_ITEM", "", "Id", RequisitionItemId);
                        }
                        if (reqdata != null)
                        {
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.NoteId = reqdata["NtsNoteId"].ToString();
                            noteTempModel1.SetUdfValue = true;
                            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            rowData["IssuedQuantity"] = rowData.ContainsKey("IssuedQuantity") && (rowData["IssuedQuantity"].ToString()!= null && rowData["IssuedQuantity"].ToString() != "null" && rowData["IssuedQuantity"].ToString() != "") ? (Convert.ToDecimal(rowData["IssuedQuantity"]) + Convert.ToDecimal(CurrentIssueQuantity)).ToString() : (0 + Convert.ToDecimal(CurrentIssueQuantity));
                            if (issueReferenceType == ImsIssueTypeEnum.Requisition)
                            {
                                if (Convert.ToDecimal(rowData["IssuedQuantity"]) >= Convert.ToDecimal(rowData["ApprovedQuantity"]))
                                {
                                    if (rowData.ContainsKey("Issued"))
                                    {
                                        rowData["Issued"] = "True";
                                    }

                                }
                            }
                            else if (issueReferenceType == ImsIssueTypeEnum.StockTransfer) 
                            {
                                if (Convert.ToDecimal(rowData["IssuedQuantity"]) == Convert.ToDecimal(rowData["TransferQuantity"]))
                                {
                                    if (rowData.ContainsKey("Issued"))
                                    {
                                        rowData["Issued"] = "True";
                                    }

                                }
                            }
                            else if (issueReferenceType == ImsIssueTypeEnum.StockAdjustment)
                            {
                                if (Convert.ToDecimal(rowData["IssuedQuantity"]) == Convert.ToDecimal(rowData["AdjustmentQuantity"]))
                                {
                                    if (rowData.ContainsKey("Issued"))
                                    {
                                        rowData["Issued"] = "True";
                                    }

                                }
                            }
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                            var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                        }
                        if (issueReferenceType == ImsIssueTypeEnum.StockTransfer)
                        {
                            // update All Issue In Stock Transfer
                            var stockTransferId = Convert.ToString(udf.IssueReferenceId);
                            var service = await _inventoryManagementBusiness.GetTransferById(stockTransferId);
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.NoteId = service.NtsNoteId;
                            noteTempModel1.SetUdfValue = true;
                            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            
                            //var data = await _inventoryManagementBusiness.GetRequisistionIssue(stockTransferId, ImsIssueTypeEnum.StockTransfer);
                            bool AllIssued = true;
                           // foreach (var issue in data) 
                           // {
                               var transferItems= await _inventoryManagementBusiness.GetTransferItemsList(stockTransferId);
                                foreach (var issueitem in transferItems)
                                {
                                    if (issueitem.Issued != "True")
                                    {
                                        AllIssued = false;
                                        break;
                                    }                                   
                                }                                                             
                               // }

                            if (rowData.ContainsKey("AllIssued"))
                            {
                                rowData["AllIssued"] = AllIssued;
                            }

                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                            var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                        }
                        // update Closing Balance
                        var itemId = rowData1.ContainsKey("ItemId") ? Convert.ToString(rowData1["ItemId"]) : "";
                        var warehouseId = rowData1.ContainsKey("WarehouseId") ? Convert.ToString(rowData1["WarehouseId"]) : "";
                        var closingBalance=await _inventoryManagementBusiness.GetClosingBalance(itemId, warehouseId);
                        await _inventoryManagementBusiness.UpdateStockClosingBalance(itemId, warehouseId, closingBalance);
                       
                    }

                }               
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateApprovedQuantityInRequisitionItem(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                // Get All Issued Items
                var items = await _inventoryManagementBusiness.GetRequisistionItemsData(viewModel.ServiceId);
                if (items.IsNotNull() && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = item.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var ItemQuantity = rowData.ContainsKey("ItemQuantity") ? Convert.ToString(rowData["ItemQuantity"]) : "0";
                         rowData["ApprovedQuantity"] = ItemQuantity;                        
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                    }

                }
              
               
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateIssueOrRequisitionOnStockAdjustment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                if (udf.StockAdjustmentType == "0")
                {
                    // Receive items
                    var noteTempModel = new ServiceTemplateViewModel();                  
                    noteTempModel.ActiveUserId = uc.UserId;
                    noteTempModel.TemplateCode = "IMS_GOODS_RECEIPT";
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();                 
                    ((IDictionary<String, Object>)exo).Add("Remark", udf.Comment);
                    ((IDictionary<String, Object>)exo).Add("ReceiveDate", udf.AdjustmentDate);
                    ((IDictionary<String, Object>)exo).Add("WarehouseId", udf.WarehouseId);
                    ((IDictionary<String, Object>)exo).Add("ReceiptType", ImsReceiptTypeEnum.StockAdjustment);
                    ((IDictionary<String, Object>)exo).Add("GoodsReceiptReferenceId", viewModel.UdfNoteTableId);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                       var items=await _inventoryManagementBusiness.GetStockAdjustmentItemsData(viewModel.UdfNoteTableId);
                        if (items.IsNotNull() && items.Count() > 0)
                        {
                            foreach (var data in items)
                            {
                                var noteTempModel1 = new NoteTemplateViewModel();
                                noteTempModel1.DataAction = DataActionEnum.Create;
                                noteTempModel1.ActiveUserId = uc.UserId;
                                noteTempModel1.TemplateCode = "IMS_GOODS_RECEIPT_ITEM";
                                var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                                dynamic exo1 = new System.Dynamic.ExpandoObject();

                                ((IDictionary<String, Object>)exo1).Add("GoodReceiptId", result.Item.UdfNoteTableId);
                                ((IDictionary<String, Object>)exo1).Add("ItemQuantity", data.AdjustmentQuantity);
                                ((IDictionary<String, Object>)exo1).Add("ItemId", data.ItemId);
                                ((IDictionary<String, Object>)exo1).Add("WarehouseId", udf.WarehouseId);
                                var user = await _noteBusiness.GetSingleById<UserViewModel, User>(uc.UserId);
                                ((IDictionary<String, Object>)exo1).Add("AdditionalInfo", "<"+result.Item.ServiceNo+"> received against Stock Adjustment: <"+viewModel.ServiceNo+"> By <"+ user .Name+ "> on <"+ udf.AdjustmentDate + ">");
                                ((IDictionary<String, Object>)exo1).Add("ReferenceHeaderItemId", data.Id); 
                                notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                notemodel1.DataAction = DataActionEnum.Create;
                                var result1 = await _noteBusiness.ManageNote(notemodel1);
                                var closingBalance = await _inventoryManagementBusiness.GetClosingBalance(data.ItemId, udf.WarehouseId);
                                await _inventoryManagementBusiness.UpdateStockClosingBalance(data.ItemId, udf.WarehouseId, closingBalance);
                                
                                
                            }
                        }
                        var serviceTempModel = new NoteTemplateViewModel();
                        serviceTempModel.NoteId = viewModel.UdfNoteId;
                        serviceTempModel.SetUdfValue = true;
                        var serviceModel = await _noteBusiness.GetNoteDetails(serviceTempModel);
                        var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                        rowData["ReceiptId"] = result.Item.UdfNoteTableId;

                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(serviceModel, data1, serviceModel.UdfNoteTableId);
                    }
                }
                else 
                {
                    #region Useful Code commented as per discussion with shafi can be use later as logic is shifted to some other place
                    //// Create Issue
                    //var noteTempModel = new ServiceTemplateViewModel();

                    //noteTempModel.ActiveUserId = uc.UserId;
                    //noteTempModel.TemplateCode = "IMS_REQUSISTION_ISSUE";
                    //noteTempModel.DataAction =DataActionEnum.Create;
                    //var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                    //dynamic exo = new System.Dynamic.ExpandoObject();
                    //((IDictionary<String, Object>)exo).Add("Remarks", udf.Comment);
                    //((IDictionary<String, Object>)exo).Add("IssuedOn", udf.AdjustmentDate);
                    //((IDictionary<String, Object>)exo).Add("WarehouseId", udf.WarehouseId);
                    //((IDictionary<String, Object>)exo).Add("IssueReferenceType", ImsIssueTypeEnum.StockAdjustment);
                    //((IDictionary<String, Object>)exo).Add("IssueReferenceId", viewModel.UdfNoteTableId);
                    //notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    //notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    //notemodel.DataAction = DataActionEnum.Create;
                    //var result = await _serviceBusiness.ManageService(notemodel);
                    //if (result.IsSuccess)
                    //{

                    //       // var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RequisitionIssueItemsViewModel>>(model.Items);

                    //            var items = await _inventoryManagementBusiness.GetStockAdjustmentItemsData(viewModel.UdfNoteTableId);
                    //            if (items.IsNotNull() && items.Count() > 0)
                    //            {
                    //                foreach (var data in items)
                    //                {
                    //            var noteTempModel1 = new NoteTemplateViewModel();
                    //            noteTempModel1.DataAction = DataActionEnum.Create;
                    //            noteTempModel1.ActiveUserId = uc.UserId;
                    //            noteTempModel1.TemplateCode = "IMS_REQUSISTION_ISSUE_ITEM";
                    //            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                    //            dynamic exo1 = new System.Dynamic.ExpandoObject();


                    //            ((IDictionary<String, Object>)exo1).Add("RequisitionIssueId", result.Item.UdfNoteTableId);
                    //            ((IDictionary<String, Object>)exo1).Add("IssuedQuantity", data.AdjustmentQuantity);
                    //            //((IDictionary<String, Object>)exo1).Add("RequisitionId", model.RequisitionId);
                    //            //((IDictionary<String, Object>)exo1).Add("RequisitionItemId", model.RequisitionItemId);

                    //            ((IDictionary<String, Object>)exo1).Add("IssuedFromStockIds", data.Id);
                    //            ((IDictionary<String, Object>)exo1).Add("ItemId", data.ItemId);
                    //            ((IDictionary<String, Object>)exo1).Add("WarehouseId", udf.WarehouseId);
                    //            var user = await _noteBusiness.GetSingleById<UserViewModel, User>(uc.UserId);
                    //            ((IDictionary<String, Object>)exo1).Add("AdditionalInfo", "<"+ result.Item.ServiceNo+"> issued against Stock Adjustment: <"+ viewModel.ServiceNo + "> By <"+ user.Name+"> on <"+udf.AdjustmentDate+">");
                    //            notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                    //            notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    //            notemodel1.DataAction = DataActionEnum.Create;
                    //            var result1 = await _noteBusiness.ManageNote(notemodel1);
                    //            var closingBalance = await _inventoryManagementBusiness.GetClosingBalance(data.ItemId, udf.WarehouseId);
                    //            await _inventoryManagementBusiness.UpdateStockClosingBalance(data.ItemId, udf.WarehouseId, closingBalance);
                    //        }
                    //            }
                    //    var serviceTempModel = new NoteTemplateViewModel();
                    //    serviceTempModel.NoteId = viewModel.UdfNoteId;
                    //    serviceTempModel.SetUdfValue = true;
                    //    var serviceModel = await _noteBusiness.GetNoteDetails(serviceTempModel);
                    //    var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    //    rowData["IssueId"] = result.Item.UdfNoteTableId;

                    //    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    //    var update = await _noteBusiness.EditNoteUdfTable(serviceModel, data1, serviceModel.UdfNoteTableId);

                    //}
                }
                #endregion
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateGoodsReceiptOnSalesReturn(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                // Receive items
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.ActiveUserId = uc.UserId;
                serTempModel.TemplateCode = "IMS_GOODS_RECEIPT";
                var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("Remark", udf.ReturnReason);
                ((IDictionary<String, Object>)exo).Add("ReceiveDate", udf.ReturnDate);
                //((IDictionary<String, Object>)exo).Add("WarehouseId", udf.WarehouseId);
                ((IDictionary<String, Object>)exo).Add("ReceiptType", ImsReceiptTypeEnum.SalesReturn);
                ((IDictionary<String, Object>)exo).Add("GoodsReceiptReferenceId", viewModel.UdfNoteTableId);
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                sermodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusiness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    var items = await _inventoryManagementBusiness.GetSalesReturnItemsList(viewModel.UdfNoteTableId);
                    if (items.IsNotNull() && items.Count() > 0)
                    {
                        foreach (var data in items)
                        {
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.DataAction = DataActionEnum.Create;
                            noteTempModel1.ActiveUserId = uc.UserId;
                            noteTempModel1.TemplateCode = "IMS_GOODS_RECEIPT_ITEM";
                            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo1).Add("GoodReceiptId", result.Item.UdfNoteTableId);
                            ((IDictionary<String, Object>)exo1).Add("ItemQuantity", data.ReturnQuantity);
                            ((IDictionary<String, Object>)exo1).Add("ItemId", data.ItemId);
                            //((IDictionary<String, Object>)exo1).Add("WarehouseId", udf.WarehouseId);
                            var user = await _noteBusiness.GetSingleById<UserViewModel, User>(uc.UserId);
                            ((IDictionary<String, Object>)exo1).Add("AdditionalInfo", "<" + result.Item.ServiceNo + "> received against Sales Return: <" + viewModel.ServiceNo + "> By <" + user.Name + "> on <" + udf.ReturnDate + ">");
                            
                            notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            notemodel1.DataAction = DataActionEnum.Create;
                            var result1 = await _noteBusiness.ManageNote(notemodel1);
                        }
                    }
                    var serviceTempModel = new NoteTemplateViewModel();
                    serviceTempModel.NoteId = viewModel.UdfNoteId;
                    serviceTempModel.SetUdfValue = true;
                    var serviceModel = await _noteBusiness.GetNoteDetails(serviceTempModel);
                    var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["ReceiptId"] = result.Item.UdfNoteTableId;

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusiness.EditNoteUdfTable(serviceModel, data1, serviceModel.UdfNoteTableId);
                }

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateGoodsReceiptOnPurchaseReturn(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                // Receive items
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.ActiveUserId = uc.UserId;
                serTempModel.TemplateCode = "IMS_GOODS_RECEIPT";
                var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("Remark", udf.ReturnReason);
                ((IDictionary<String, Object>)exo).Add("ReceiveDate", udf.ReturnDate);
                //((IDictionary<String, Object>)exo).Add("WarehouseId", udf.WarehouseId);
                ((IDictionary<String, Object>)exo).Add("ReceiptType", ImsReceiptTypeEnum.SalesReturn);
                ((IDictionary<String, Object>)exo).Add("GoodsReceiptReferenceId", viewModel.UdfNoteTableId);
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                sermodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusiness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    var items = await _inventoryManagementBusiness.GetPurchaseReturnItemsList(viewModel.UdfNoteTableId);
                    if (items.IsNotNull() && items.Count() > 0)
                    {
                        foreach (var data in items)
                        {
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.DataAction = DataActionEnum.Create;
                            noteTempModel1.ActiveUserId = uc.UserId;
                            noteTempModel1.TemplateCode = "IMS_GOODS_RECEIPT_ITEM";
                            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo1).Add("GoodReceiptId", result.Item.UdfNoteTableId);
                            ((IDictionary<String, Object>)exo1).Add("ItemQuantity", data.ReturnQuantity);
                            ((IDictionary<String, Object>)exo1).Add("ItemId", data.ItemId);
                            //((IDictionary<String, Object>)exo1).Add("WarehouseId", udf.WarehouseId);
                            var user = await _noteBusiness.GetSingleById<UserViewModel, User>(uc.UserId);
                            ((IDictionary<String, Object>)exo1).Add("AdditionalInfo", "<" + result.Item.ServiceNo + "> received against Sales Return: <" + viewModel.ServiceNo + "> By <" + user.Name + "> on <" + udf.ReturnDate + ">");

                            notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            notemodel1.DataAction = DataActionEnum.Create;
                            var result1 = await _noteBusiness.ManageNote(notemodel1);
                        }
                    }
                    var serviceTempModel = new NoteTemplateViewModel();
                    serviceTempModel.NoteId = viewModel.UdfNoteId;
                    serviceTempModel.SetUdfValue = true;
                    var serviceModel = await _noteBusiness.GetNoteDetails(serviceTempModel);
                    var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["ReceiptId"] = result.Item.UdfNoteTableId;

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusiness.EditNoteUdfTable(serviceModel, data1, serviceModel.UdfNoteTableId);
                }

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateDeliveredQuantityInRequisitionItem(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                // Get All Issued Items
                var items = await _inventoryManagementBusiness.GetDeliveryItemsList(viewModel.UdfNoteTableId);
                if (items.IsNotNull() && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = item.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var DeliveredQuantity = rowData1.ContainsKey("DeliveredQuantity") ? Convert.ToString(rowData1["DeliveredQuantity"]) : "0";
                        var RequisitionIssueItemId = rowData1.ContainsKey("IssuedItemsId") ? Convert.ToString(rowData1["IssuedItemsId"]) : "";
                        // var IssuedFromStockIds = rowData1.ContainsKey("IssuedFromStockIds") ? Convert.ToString(rowData1["IssuedFromStockIds"]) : null;
                        var requisitionIssueItem = await _inventoryManagementBusiness.GetRequisistionIssueItemsById(RequisitionIssueItemId);
                        var requisistionItem = await _inventoryManagementBusiness.GetRequisitionItemById(requisitionIssueItem.ReferenceHeaderItemId);
                        var noteTempModel1 = new NoteTemplateViewModel();
                        noteTempModel1.SetUdfValue = true;
                        noteTempModel1.NoteId = requisistionItem.NtsNoteId;
                        var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                        var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        rowData["DeliveredQuantity"] = rowData.ContainsKey("DeliveredQuantity") ? (Convert.ToDecimal(rowData["DeliveredQuantity"]) + Convert.ToDecimal(DeliveredQuantity)).ToString() : (0 + Convert.ToDecimal(DeliveredQuantity));
                        
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);


                    }

                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        //public async Task<CommandResult<ServiceTemplateViewModel>> CreateIssueAndReceiptOnStockTransfer(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        //{
        //    var _noteBusiness = sp.GetService<INoteBusiness>();
        //    var _serviceBusiness = sp.GetService<IServiceBusiness>();
        //    var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
        //    var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();
        //    var _lovBusiness = sp.GetService<ILOVBusiness>();
        //    if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
        //    {
        //        //// Create Issue
        //        var serTempModel = new ServiceTemplateViewModel();

        //        serTempModel.ActiveUserId = uc.UserId;
        //        serTempModel.TemplateCode = "IMS_REQUSISTION_ISSUE";
        //        serTempModel.DataAction = DataActionEnum.Create;
        //        var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);
        //        dynamic exo = new System.Dynamic.ExpandoObject();
        //        ((IDictionary<String, Object>)exo).Add("Remarks", udf.TransferReason);
        //        ((IDictionary<String, Object>)exo).Add("IssuedOn", udf.TransferDate);
        //        ((IDictionary<String, Object>)exo).Add("WarehouseId", udf.FromWarehouseId);
        //        ((IDictionary<String, Object>)exo).Add("IssueReferenceType", ImsIssueTypeEnum.StockTransfer);
        //        ((IDictionary<String, Object>)exo).Add("IssueReferenceId", viewModel.UdfNoteTableId);
        //        sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
        //        sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
        //        sermodel.DataAction = DataActionEnum.Create;
        //        var result = await _serviceBusiness.ManageService(sermodel);

        //        if (result.IsSuccess)
        //        {
        //            //var items = await _inventoryManagementBusiness.GetTransferItemsList(viewModel.UdfNoteTableId);
        //            //if (items.IsNotNull() && items.Count > 0)
        //            //{
        //            //    foreach (var data in items)
        //            //    {
        //            //        var noteTempModel = new NoteTemplateViewModel();
        //            //        noteTempModel.DataAction = DataActionEnum.Create;
        //            //        noteTempModel.ActiveUserId = uc.UserId;
        //            //        noteTempModel.TemplateCode = "IMS_REQUSISTION_ISSUE_ITEM";
        //            //        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
        //            //        dynamic exo1 = new System.Dynamic.ExpandoObject();

        //            //        ((IDictionary<String, Object>)exo1).Add("RequisitionIssueId", result.Item.UdfNoteTableId);
        //            //        ((IDictionary<String, Object>)exo1).Add("IssuedQuantity", data.TransferQuantity);
        //            //        //((IDictionary<String, Object>)exo1).Add("RequisitionId", model.RequisitionId);
        //            //        //((IDictionary<String, Object>)exo1).Add("RequisitionItemId", model.RequisitionItemId);

        //            //        ((IDictionary<String, Object>)exo1).Add("IssuedFromStockIds", data.Id);
        //            //        ((IDictionary<String, Object>)exo1).Add("ItemId", data.ItemId);
        //            //        ((IDictionary<String, Object>)exo1).Add("WarehouseId", udf.FromWarehouseId);
        //            //        var user = await _noteBusiness.GetSingleById<UserViewModel, User>(uc.UserId);
        //            //        ((IDictionary<String, Object>)exo1).Add("AdditionalInfo", "<" + result.Item.ServiceNo + "> issued against Stock Transfer: <" + viewModel.ServiceNo + "> By <" + user.Name + "> on <" + udf.TransferDate + ">");
        //            //        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
        //            //        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
        //            //        notemodel.DataAction = DataActionEnum.Create;
        //            //        var result2 = await _noteBusiness.ManageNote(notemodel);
        //            //        //var closingBalance = await _inventoryManagementBusiness.GetClosingBalance(data.ItemId, udf.FromWarehouseId);
        //            //        //await _inventoryManagementBusiness.UpdateStockClosingBalance(data.ItemId, udf.FromWarehouseId, closingBalance);
        //            //    }
        //            //}

        //            var serviceTempModel = new NoteTemplateViewModel();
        //            serviceTempModel.NoteId = viewModel.UdfNoteId;
        //            serviceTempModel.SetUdfValue = true;
        //            var serviceModel = await _noteBusiness.GetNoteDetails(serviceTempModel);
        //            var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

        //            rowData["IssueId"] = result.Item.UdfNoteTableId;

        //            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
        //            var update = await _noteBusiness.EditNoteUdfTable(serviceModel, data1, serviceModel.UdfNoteTableId);

        //            //Create Receipt
        //            var serTempModel1 = new ServiceTemplateViewModel();
        //            serTempModel1.ActiveUserId = uc.UserId;
        //            serTempModel1.TemplateCode = "IMS_GOODS_RECEIPT";
        //            var sermodel1 = await _serviceBusiness.GetServiceDetails(serTempModel1);
        //            dynamic exo2 = new System.Dynamic.ExpandoObject();
        //            ((IDictionary<String, Object>)exo2).Add("Remark", udf.TransferReason);
        //            ((IDictionary<String, Object>)exo2).Add("ReceiveDate", udf.TransferDate);
        //            ((IDictionary<String, Object>)exo2).Add("WarehouseId", udf.ToWarehouseId);
        //            ((IDictionary<String, Object>)exo2).Add("ReceiptType", ImsReceiptTypeEnum.StockTransfer);
        //            ((IDictionary<String, Object>)exo2).Add("GoodsReceiptReferenceId", viewModel.UdfNoteTableId);
        //            sermodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo2);
        //            sermodel1.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
        //            sermodel1.DataAction = DataActionEnum.Create;
        //            var result1 = await _serviceBusiness.ManageService(sermodel1);
        //            if (result1.IsSuccess)
        //            {
        //                var items = await _inventoryManagementBusiness.GetTransferItemsList(viewModel.UdfNoteTableId);
        //                if (items.IsNotNull() && items.Count() > 0)
        //                {
        //                    foreach (var data in items)
        //                    {
        //                        var noteTempModel1 = new NoteTemplateViewModel();
        //                        noteTempModel1.DataAction = DataActionEnum.Create;
        //                        noteTempModel1.ActiveUserId = uc.UserId;
        //                        noteTempModel1.TemplateCode = "IMS_GOODS_RECEIPT_ITEM";
        //                        var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
        //                        dynamic exo3 = new System.Dynamic.ExpandoObject();

        //                        ((IDictionary<String, Object>)exo3).Add("GoodReceiptId", result1.Item.UdfNoteTableId);
        //                        ((IDictionary<String, Object>)exo3).Add("ItemQuantity", data.TransferQuantity);
        //                        ((IDictionary<String, Object>)exo3).Add("BalanceQuantity", data.TransferQuantity);
        //                        ((IDictionary<String, Object>)exo3).Add("ItemId", data.ItemId);
        //                        ((IDictionary<String, Object>)exo3).Add("WarehouseId", udf.ToWarehouseId);
        //                        var user = await _noteBusiness.GetSingleById<UserViewModel, User>(uc.UserId);
        //                        ((IDictionary<String, Object>)exo3).Add("AdditionalInfo", "<" + result.Item.ServiceNo + "> received against Stock Transfer: <" + viewModel.ServiceNo + "> By <" + user.Name + "> on <" + udf.TransferDate + ">");
        //                        ((IDictionary<String, Object>)exo3).Add("ReferenceHeaderItemId", data.StockTransferItemId);
        //                        notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo3);
        //                        notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
        //                        notemodel1.DataAction = DataActionEnum.Create;
        //                        var result2 = await _noteBusiness.ManageNote(notemodel1);
        //                        //var closingBalance = await _inventoryManagementBusiness.GetClosingBalance(data.ItemId, udf.ToWarehouseId);
        //                        //await _inventoryManagementBusiness.UpdateStockClosingBalance(data.ItemId, udf.ToWarehouseId, closingBalance);
        //                    }
        //                }
        //                var serviceTempModel1 = new NoteTemplateViewModel();
        //                serviceTempModel1.NoteId = viewModel.UdfNoteId;
        //                serviceTempModel1.SetUdfValue = true;
        //                var serviceModel1 = await _noteBusiness.GetNoteDetails(serviceTempModel1);
        //                var rowData1 = serviceModel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

        //                rowData1["ReceiptId"] = result1.Item.UdfNoteTableId;

        //                var data2 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
        //                var update1 = await _noteBusiness.EditNoteUdfTable(serviceModel, data2, serviceModel.UdfNoteTableId);
        //            }
        //        }

        //    }
        //    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        //}


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateRequisitionItemPOQuantity(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _inventoryBusiness = sp.GetService<IInventoryManagementBusiness>();

            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {                
                    var PoItems = await _inventoryBusiness.ReadPOItemsData(Convert.ToString(viewModel.UdfNoteTableId));
                    foreach (var item in PoItems) 
                    {
                    var req = await _inventoryBusiness.GetRequisitionItemById(Convert.ToString(item.RequisitionItemId));
                                     
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = req.NoteId;
                        noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    var POQuantity = rowData.ContainsKey("POQuantity") ? Convert.ToString(rowData["POQuantity"]) : "";
                    rowData["POQuantity"] = (Convert.ToInt32(rowData["POQuantity"]) + Convert.ToInt32(item.ItemQuantity)).ToString();
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    
                }             
            }
            else if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || viewModel.ServiceStatusCode == "SERVICE_STATUS_REJECT")
            {
                var PoItems = await _inventoryBusiness.ReadPOItemsData(Convert.ToString(viewModel.UdfNoteTableId));
                foreach (var item in PoItems)
                {
                    var req = await _inventoryBusiness.GetRequisitionItemById(Convert.ToString(item.RequisitionItemId));
                    if (req.IsNotNull())
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = req.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var POQuantity = rowData.ContainsKey("POQuantity") ? Convert.ToString(rowData["POQuantity"]) : "0";
                        rowData["POQuantity"] = (Convert.ToInt32(POQuantity) - Convert.ToInt32(item.ItemQuantity)).ToString();
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    }                   
                }

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdatePOItemReceivedQuantity(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _inventoryBusiness = sp.GetService<IInventoryManagementBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var goodreceiptItems = await _inventoryBusiness.GetGoodReceiptItemsList(viewModel.UdfNoteTableId);
                foreach (var item in goodreceiptItems) 
                {
                    var req = await _inventoryBusiness.GetPOItemById(Convert.ToString(item.ReferenceHeaderItemId));
                    if (req.IsNotNull()) 
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = req.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var POQuantity = rowData.ContainsKey("ReceivedQuantity") ? Convert.ToString(rowData["ReceivedQuantity"]) : "";
                        rowData["ReceivedQuantity"] = (Convert.ToInt32(rowData["ReceivedQuantity"]) + Convert.ToInt32(item.ItemQuantity)).ToString();
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    }                   

                    // update Closing Balance
                    var itemId = Convert.ToString(item.Id);
                    var warehouseId = Convert.ToString(item.WarehouseId);
                    var closingBalance = await _inventoryBusiness.GetClosingBalance(itemId, warehouseId);
                    await _inventoryBusiness.UpdateStockClosingBalance(itemId, warehouseId, closingBalance);
                }
            
            }
            else if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || viewModel.ServiceStatusCode == "SERVICE_STATUS_REJECT")
            {
                var goodreceiptItems = await _inventoryBusiness.GetGoodReceiptItemsList(viewModel.UdfNoteTableId);
                foreach (var item in goodreceiptItems)
                {
                    var req = await _inventoryBusiness.GetPOItemById(Convert.ToString(item.ReferenceHeaderItemId));
                    if (req.IsNotNull())
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = req.NoteId;
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var ReceivedQuantity = rowData.ContainsKey("ReceivedQuantity") ? Convert.ToString(rowData["ReceivedQuantity"]) : "0";
                        rowData["ReceivedQuantity"] = (Convert.ToInt32(ReceivedQuantity) - Convert.ToInt32(item.ItemQuantity)).ToString();
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    }

                    // update Closing Balance
                    var itemId = Convert.ToString(item.Id);
                    var warehouseId = Convert.ToString(item.WarehouseId);
                    var closingBalance = await _inventoryBusiness.GetClosingBalance(itemId, warehouseId);
                    await _inventoryBusiness.UpdateStockClosingBalance(itemId, warehouseId, closingBalance);
                }

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }

}
