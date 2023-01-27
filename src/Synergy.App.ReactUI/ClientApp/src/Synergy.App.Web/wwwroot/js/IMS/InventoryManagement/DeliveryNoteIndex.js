var columnDefs = [
    {
        name: "ServiceNo",
        title: "Delivery No",
        type:"text"
    },
  
    {
        name: "NameScopeOfWork",
        title: "Name Scope Of Work",
        type: "text"
    },
    {
        name: "DeliveryOn",
        title: "Delivery On",
        type: "text",
        itemTemplate: function (value, item) {
            var d = new Date(value);
            return d.getDate() + "." + (d.getMonth() + 1) + "." + + d.getFullYear();

        }
    },
    //{
    //    name: "VehicleNo",
    //    title: "Vehicle No",
    //    type: "text"
    //},
    //{
    //    name: "PIN",
    //    title: "PIN",
    //    type: "text"
    //},
    //{
    //    name: "ShippingAddress",
    //    title: "Shipping Address",
    //    type: "text"
    //},
    //{
    //    name: "GSTIN",
    //    title: "GSTIN",
    //    type: "text"
    //},
    {
        name: "ServiceStatusName",
        title: "Status",
        type: "text"
        //cellRenderer: params => {
        //    return statusObj[params.value]; //only for enum
        //}
    },
    {
        name: "CreatedBy",
        title: "Deliver By",
        type: "text"
    },
    {
        title: "Actions",
        name: "Id",
        type: "text",
        itemTemplate: function (value, item) {
            
            return "<div class='btn-group grid-menu' id='tree-menudeliverynote' data-idvalue='" + value + "' data-serviceid=\"" + item.ServiceId + "\" data-status=\"" + item.ServiceStatusCode + "\" data-serviceno=\"" + item.ServiceNo + "\" data-issue=\"" + item.Issued + "\" ><i class='fas fa-ellipsis-v'></i></div>"
        }
    }
];
function onRequisitionchange(e) {
    debugger
    var dataItem = $("#RequisitionId").data("kendoDropDownList").dataItem();
    if (dataItem != null && dataItem.Id != "")  {
        $("#GridArea").show();
        
        
        $("#DeliveryNoteGrid").jsGrid({
            width: "100%",
            height: "600px",

            inserting: false,
            editing: false,
            sorting: true,
            paging: true,
            autoload: true,
            pageSize: 14,
            pageButtonCount: 5,
            deleteConfirm: "Do you really want to delete client?",

            //data: clients,
            controller: {
                loadData: function (filter) {
                    var sd = kendo.toString($("#FromDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
                    var ed = kendo.toString($("#ToDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
                    return $.ajax({
                        type: "GET",
                        url: "/IMS/InventoryManagement/ReadDeliveryNoteData?ItemHead=" + $("#ItemHead1").data("kendoDropDownList").value() + "&From=" + sd + "&To=" + ed + "&requisitionId=" + dataItem.Id,
                        data: filter,
                        dataType: "json"
                    });
                },

                //deleteItem: function (item) {
                //    onDeleteItem(item.NoteId);
                //}
            },
            //onItemEditing: function (e) {
            //    if (e.grid._container.find(".jsgrid-edit-row")[0] != undefined) {
            //        //  
            //        e.grid.updateItem();
            //    }
            //},

            fields: columnDefs,
            //rowRenderer: function (item) {
            //    
            //    var row = $("<tr>");
            //    var addressesGrid = $('<tr class="random">').hide();
            //     //$.ajax({
            //     //   type: "GET",
            //     //   url: "/IMS/InventoryManagement/GetRequisistionIssueItemsByRequisitionId?requisitionId=" + item.ServiceId,                
            //     //   dataType: "json",
            //     //   success: function (response)
            //     //   {

            //            addressesGrid.jsGrid({
            //                width: "100%",
            //                height: "auto",                      
            //                //data: response,
            //                autoload: true,
            //                controller: {
            //                    loadData: function (filter) {

            //                        return $.ajax({
            //                            type: "GET",
            //                            url: "/IMS/InventoryManagement/GetRequisistionIssue?requisitionId=" + item.ServiceId,
            //                            data: filter,
            //                            dataType: "json"
            //                        });
            //                    },

            //                    //deleteItem: function (item) {
            //                    //    onDeleteItem(item.NoteId);
            //                    //}
            //                },
            //                fields: [

            //                    {
            //                        name: "IssueType",
            //                        title: "Issue Type",
            //                        type: "text",

            //                    },
            //                    //{
            //                    //    field: "ProposalDate",
            //                    //    headerName: "Proposal Date"
            //                    //},
            //                    {
            //                        name: "IssuedOn",
            //                        title: "Issued On",
            //                        type: "text",

            //                    },
            //                    {
            //                        name: "IssueTo",
            //                        title: "IssueTo",
            //                        type: "text",

            //                    },
            //                    {
            //                        name: "Remarks",
            //                        title: "Remarks",
            //                        type: "text",

            //                    },
            //                    {
            //                        title: "Actions",
            //                        name: "ServiceId",
            //                        type: "text",
            //                        itemTemplate: function (value, item) {
            //                            
            //                            return "<div class='btn-group grid-menu' id='tree-menuissuerequisitionitem' data-idvalue='" + value + "' ><i class='fas fa-ellipsis-v'></i></div>"
            //                        }
            //                    }
            //                    //{
            //                    //    name: "IssuedQuantity",
            //                    //    title: "Issued Quantity",
            //                    //    type: "number",


            //                    //},
            //                    //{
            //                    //    name: "BalanceQuantity",
            //                    //    title: "Balance Quantity",
            //                    //    type: "number", editcss: "team-edit",


            //                    //},
            //                    //{
            //                    //    name: "CurrentIssueQuantity",
            //                    //    title: "Current Issue Quantity",
            //                    //    type: "number",

            //                    //},


            //                ]
            //            })
            //    
            //    var grid = $("#RequisitionOrderGrid").data("JSGrid")
            //    var fields = grid.fields;
            //        items = Object.keys(item)
            //    fields.forEach(function (key) {
            //                
            //        if (key.name != fields[fields.length - 1].name) {

            //            var cell = $("<td>").addClass("jsgrid-cell").append(item[key.name])
            //            row.append(cell);

            //        }
            //        else
            //        {
            //            var cell = $("<td>").addClass("jsgrid-cell").append("<div class='btn-group grid-menu' id='tree-menuissuerequisition' data-idvalue='" + item.ServiceId + "' data-status=\"" + item.ServiceStatusCode + "\" data-serviceno=\"" + item.ServiceNo + "\" data-issue=\"" + item.Issued + "\" ><i class='fas fa-ellipsis-v'></i></div>");
            //            row.append(cell);
            //        }
            //            })
            //            row.click(function () {
            //                addressesGrid.toggle();
            //            })
            //            return row.add(addressesGrid);
            //        //}
            //   // });

            //}
        });
    }
    else {

        $("#GridArea").hide();
        $("#DeliveryNoteGrid").html("");
    }
}
$(function () {
 
    //var sd = kendo.toString($("#FromDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
    //var ed = kendo.toString($("#ToDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
    $("#RequisitionId").kendoDropDownList({
            dataTextField: "ServiceNo",
            dataValueField: "Id",
            optionLabel: "--All--",
            change: onRequisitionchange,
            dataSource: {
                transport: {
                    read: {
                        url: "/Ims/InventoryManagement/ReadRequisitionData",//?ItemHead=" + $("#ItemHead1").data("kendoDropDownList").value() + "&From=" + sd+"&To="+ed,
                    }
                }
            }
        });
        $("#ItemHead1").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "--All--",
            dataSource: {
                transport: {
                    read: {
                        url: "/Cms/LOV/GetListOfValueList?type=IMS_ITEM_TYPE",
                    }
                }
            }
        });
    var d = new Date();
    d.setDate(d.getDate() - 6);
        $("#FromDate").kendoDatePicker({
            value: d,
            format: "dd.MM.yyyy"
        });
        $("#ToDate").kendoDatePicker({
            value: new Date(),
            format: "dd.MM.yyyy"
        });
        $("#Customer").kendoDropDownList({
            dataTextField: "CustomerName",
            dataValueField: "Id",
            optionLabel: "--All--",

            filter: "contains",
            dataSource: {
                transport: {
                    read: {
                        url: "/IMS/InventoryManagement/GetCustomerList",
                    }
                }
            }
        });
        $.contextMenu({
            selector: '#tree-menudeliverynote',
            trigger: 'left',
            build: function ($trigger, e) {


                var id = $trigger.data('idvalue');
                var serno = $trigger.data('serviceno');
                var status = $trigger.data('status');
                var serviceId = $trigger.data('serviceid');
                // var issued = $trigger.data('issue');
                switch (0) {
                    case 0:
                        // 
                        var Items;
                        if (status == "SERVICE_STATUS_DRAFT") {
                            Items = {

                                "acknowledge": { name: "Acknowledge", icon: "fa-regular fa-message-exclamation" },
                                "deliveryNote": { name: "Delivery Note", icon: "fas fa-file-lines" },
                                "submit": { name: "Submit", icon: "fa-regular fa-arrow-right-to-bracket" },
                            };                          
                        }
                        else {
                            Items = {

                                "acknowledge": { name: "Acknowledge", icon: "fa-regular fa-message-exclamation" },
                                "deliveryNote": { name: "Delivery Note", icon: "fas fa-file-lines" },

                            };
                        }
                        

                        return {
                            callback: function (key, options) {
                                switch (key) {
                                    case 'acknowledge':
                                        onAccknowledgement(id);
                                        break;
                                    case 'submit':
                                        onSubmit(serviceId);
                                        break;


                                    case 'deliveryNote':
                                        onDeliveryNoteReport(id);
                                        break;

                                    default:
                                }
                            },
                            items: Items
                        };


                }
            }
        });
 
    
    //$("#RequisitionStatus").kendoDropDownList({
    //    dataTextField: "Name",
    //    dataValueField: "Id",
    //    optionLabel: "--All--",

    //    filter: "contains",
    //    dataSource: {
    //        transport: {
    //            read: {
    //                url: "/Cms/LOV/GetListOfValueList?type=LOV_SERVICE_STATUS",
    //            }
    //        }
    //    }
    //});
  
  
    //$.contextMenu({
    //    selector: '#tree-menuissuerequisitionitem',
    //    trigger: 'left',
    //    build: function ($trigger, e) {


    //        var id = $trigger.data('idvalue');
    //       // var serno = $trigger.data('serviceno');
    //       // var status = $trigger.data('status');
    //        // var issued = $trigger.data('issue');
    //        switch (0) {
    //            case 0:
    //                // 
    //                var Items;

    //                Items = {
    //                    "items": { name: "Issued Items", icon: "fas fa-edit" },
                       
    //                };

    //                return {
    //                    callback: function (key, options) {
    //                        switch (key) {
    //                            case 'items':
    //                                ViewIssuedItems(id)
    //                                break;

                              

    //                            default:
    //                        }
    //                    },
    //                    items: Items
    //                };


    //        }
    //    }
    //});
  

  
    
});
function onEdit(id)
{
    var win = GetMainWindow();
    win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?id=' + id;
    win.OpenWindow({ Title: 'Edit Requisition', Width: 1000, Height: 800 });
    return false;
}
function OnSearch(e) {
    //var grid = $("#DeliveryNoteGrid").data("JSGrid");
    //grid.render();
    var sd = kendo.toString($("#FromDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
    var ed = kendo.toString($("#ToDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
    $("#RequisitionId").data("kendoDropDownList").dataSource.read({
        ItemHead: $("#ItemHead1").data("kendoDropDownList").value(),
        From: sd,
        To:ed
    });
    var source = $("#RequisitionId").data("kendoDropDownList").dataSource.data();
    if (source.length > 0) { $("#ItemsArea").show() } else { $("#ItemsArea").hide(); $("#GridArea").hide();}
}
function onDeliveryNoteReport(udfnotetableid) {
    var portalId = $("#GlobalPortalId").val();
    var url = '/Cms/FastReport?rptName=IMS_DeliveryNote&lo=@LayoutModeEnum.Popup&portalId=' + portalId + '&rptUrl=ims/query/GetDeliveryNoteDetails?deliveryNoteId=' + udfnotetableid;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Delivery Note Report', Width: 800, Height: 700 });
}

function onSubmit(id) {
    var flag = confirm('Do you really want to Submit?');

    if (flag) {
        ShowLoader($("#cms-content"));
        $.ajax({
            url: '/Ims/InventoryManagement/SubmitDeliveryNote?ServiceId=' + id,
            type: 'POST',
            data: {},
            dataType: 'json',
            success: function (result) {
                HideLoader($("#cms-content"));
                if (result.success) {
                    OnSearch();
                    kendo.alert("Submitted Successfully.");
                } else {
                    OnSearch();
                   // var err = result.errors.BinderCountError.errors[0];
                    kendo.alert(result.errors);
                }
            },
            error: function (ert) {
                kendo.alert("Please submit again");
               // getDirectSalesData();
            }
        });
        return false;
    }
}

function onAccknowledgement(id)
{
    var win = GetMainWindow();
    win.iframeOpenUrl = "/Ims/InventoryManagement/DeliveryNoteAcknowledgement?deliveryNoteId="+id;
    win.OpenWindow({ Title: 'Delivery Acknowledgement', Width: 800, Height: 700 });
}

