var columnDefss = [
    {
        name: "ServiceNo",
        title: "Req. No",
        type:"text"
    },
    {
        name: "RequisitionDate",
        title: "Req. Date",
        type: "text",
        itemTemplate: function (value, item) {
            var d = new Date(value);
            return d.getDate() + "." + (d.getMonth()+1) + "." + + d.getFullYear();
            //return moment(value).format('dd/MM/yyyy')
        }
    },
    {
        name: "RequisitionParticular",
        title: "Particular",
        type: "text"
    },
    {
        name: "RequisitionValue",
        title: "Req. Value",
        type: "text"
    },
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
        title: "Prepared By",
        type: "text"
    },
    {
        title: "Actions",
        name: "Id",
        type: "text",
        itemTemplate: function (value, item) {
            
            return "<div class='btn-group grid-menu' id='tree-menuissuerequisition' data-idvalue='" + value + "' data-status=\"" + item.ServiceStatusCode + "\" data-serviceno=\"" + item.ServiceNo + "\" data-issue=\"" + item.Issued + "\"  ><i class='fas fa-ellipsis-v'></i></div>"
        }
    }
];

$(function () {
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
    $("#WarehouseId").kendoDropDownList({
        dataTextField: "WarehouseName",
        dataValueField: "Id",

        optionLabel: "--Select--",

        dataSource: {
            transport: {
                read: {
                    url: "/IMS/InventoryManagement/GetWarehouseList",
                }
            }
        }
    });  
    var d = new Date();
    d.setDate(d.getDate() - 6);
    $("#FromDate").kendoDatePicker({
        value: d,
        format:"dd.MM.yyyy"
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
    $("#RequisitionStatus").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        optionLabel: "--All--",

        filter: "contains",
        dataSource: {
            transport: {
                read: {
                    url: "/Cms/LOV/GetListOfValueList?type=LOV_SERVICE_STATUS",
                }
            }
        }
    });
  
    $.contextMenu({
        selector: '#tree-menuissuerequisition',
        trigger: 'left',
        build: function ($trigger, e) {


            var id = $trigger.data('idvalue');
            var serno = $trigger.data('serviceno');
            var status = $trigger.data('status');
            //var issuedqty = $trigger.data('issuedqty');
            //var deliveredqty = $trigger.data('deliveredqty');
            switch (0) {
                case 0:
                   
                    var Items;
                    //if (issuedqty == deliveredqty) {
                    //    Items = {
                            
                    //        "issueItems": { name: "Manage Requisition Issue", icon: "fa-regular fa-message-exclamation" },
                    //        //"deliveryNote": { name: "Delivery Note", icon: "fa-light fa-memo" },
                    //    };
                    //}
                    //else
                    //{
                        Items = {
                            //"proposal": { name: "Proposal", icon: "fas fa-edit" },
                            "issueItems": { name: "Manage Requisition Issue", icon: "fa-regular fa-message-exclamation" },
                            "deliveryNote": { name: "Delivery Note", icon: "fa-light fa-memo" },
                        };
                    //}
                        
                   
                    return {
                        callback: function (key, options) {
                            switch (key) {
                                case 'proposal':
                                    //onAddItem(id, serno);
                                    break;

                                case 'issueItems':
                                    OnIssueItem(id, serno);
                                    break;
                               
                                case 'deliveryNote':
                                    OnDeliveryNote(id);
                                    break;

                                default:
                            }
                        },
                        items: Items
                    };


            }
        }
    });
   
    $("#RequisitionsIssueGrid").jsGrid({
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
                    url: "/IMS/InventoryManagement/ReadIssueRequisitionData?ItemHead=" + $("#ItemHead1").data("kendoDropDownList").value() + "&From=" + sd + "&To=" + ed,
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
       
        fields: columnDefss,
        //rowRenderer: function (item) {
            
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
            
        //    var grid = $("#RequisitionOrderGrid").data("JSGrid")
        //    var fields = grid.fields;
        //        items = Object.keys(item)
        //    fields.forEach(function (key) {
                        
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

  
    
});
function onEdit(id)
{
    var win = GetMainWindow();
    win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?id=' + id;
    win.OpenWindow({ Title: 'Edit Requisition', Width: 1000, Height: 800 });
    return false;
}
function OnIssueItem(id, serno) {
    if ($("#WarehouseId").data("kendoDropDownList").value() == "" || $("#WarehouseId").data("kendoDropDownList").value() == null || $("#WarehouseId").data("kendoDropDownList").value() == undefined) {
        alert("Please select Warehouse");
    }
    else
    {
        var win = GetMainWindow();
        win.iframeOpenUrl = '/IMS/InventoryManagement/RequisitionIssueItems?requisitionId=' + id + '&warehouseId=' + $("#WarehouseId").data("kendoDropDownList").value() +'&issuetype=0';
        win.OpenWindow({ Title: 'Issue Item For Requisition', Width: 1200, Height: 800 });
        return false;
    }
   
}


function OnDeliveryNote(id) {
    //if ($("#ItemHead1").data("kendoDropDownList").value() == "" || $("#ItemHead1").data("kendoDropDownList").value() == null || $("#ItemHead1").data("kendoDropDownList").value() == undefined) {
    //    alert("Please select Item Head");
    //}
    //else {
    var win = GetMainWindow();
    win.iframeOpenUrl = '/IMS/InventoryManagement/ManageDeliveryNote?id=' + id ;
    win.OpenWindow({ Title: 'Delivery Note', Width: 1000, Height: 800 });
    return false;
    //}
}


function OnSearch2(e) {
    var grid = $("#RequisitionsIssueGrid").data("JSGrid");
    grid.render();
  
}

function AddRequisition()
{
    if ($("#ItemHead1").data("kendoDropDownList").value() == "" || $("#ItemHead1").data("kendoDropDownList").value() == null || $("#ItemHead1").data("kendoDropDownList").value() == undefined) {
        alert("Please select Item Head");
    }
    else
    {
        var win = GetMainWindow();
        win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?itemHead=' + $("#ItemHead1").data("kendoDropDownList").value();
        win.OpenWindow({ Title: 'Add New Requisition', Width: 1000, Height: 800 });
    }
    return false;
}
function OnSubmit(id) {
    var flag = confirm('Do you really want to Submit?');

    if (flag) {
        $.ajax({
            url: '/IMS/InventoryManagement/SubmitRequisition?ServiceId=' + id,
            type: 'POST',
            data: {},
            dataType: 'json',
            success: function (result) {
                
                if (result.success) {
                    var grid = $("#RequisitionsIssueGrid").data("JSGrid");
                    grid.render();
                    kendo.alert("Submitted Successfully.");
                } else {
                    var grid = $("#RequisitionsIssueGrid").data("JSGrid");
                    grid.render();
                    var err = result.errors.BinderCountError.errors[0];
                    kendo.alert(err);
                }
            },
            error: function (ert) {
                var grid = $("#RequisitionsIssueGrid").data("JSGrid");
                grid.render();
            }
        });
        return false;
    }
}


