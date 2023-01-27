var columnDefs = [
    {
        field: "ServiceNo", cellRenderer: 'agGroupCellRenderer',
        headerName: "Req. No"
    },
    {
        field: "RequisitionDate",
        headerName: "Req. Date",
        cellRenderer: (data) => {
            return moment(data.value).format('dd/MM/yyyy')
        }
    },
    {
        field: "RequisitionParticular",
        headerName: "Particular"
    },
    {
        field: "RequisitionValue",
        headerName: "Req. Value"
    },
    {
        field: "ServiceStatusName",
        headerName: "Status",
        //cellRenderer: params => {
        //    return statusObj[params.value]; //only for enum
        //}
    },
    {
        field: "CreatedBy",
        headerName: "Prepared By"
    },
    {
        headerName: "Actions",
        field: "ServiceId",
        cellRenderer: params => {
            debugger;
            return "<div class='btn-group grid-menu' id='tree-menurequisition' data-idvalue='" + params.value + "' data-status=\"" + params.data.ServiceStatusCode + "\" data-serviceno=\"" + params.data.ServiceNo + "\" ><i class='fas fa-ellipsis-v'></i></div>"
        }
    }
];


$(function () {
    $("#ItemHead").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        optionLabel: "--Select--",    
        dataSource: {
            transport: {
                read: {
                    url: "/Cms/LOV/GetListOfValueList?type=IMS_ITEM_TYPE",
                }
            }
        }
    });
    $("#RequisitionFromDate").kendoDatePicker({
        value: new Date()
    });
    $("#RequisitionToDate").kendoDatePicker({
        value: new Date()
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
    getRequisitionData();
    $.contextMenu({
        selector: '#tree-menurequisition',
        trigger: 'left',
        build: function ($trigger, e) {


            var id = $trigger.data('idvalue');
            var serno = $trigger.data('serviceno');
            var status = $trigger.data('status');

            switch (0) {
                case 0:
                    debugger;
                    var Items;
                    if (status == "SERVICE_STATUS_DRAFT") {
                        Items = {
                            "edit": { name: "Edit", icon: "fas fa-edit" },
                            "items": { name: "Item", icon: "fas fa-edit" },
                            "submit": { name: "Submit", icon: "fas fa-edit" },
                        };
                    }
                    //if (status == "SERVICE_STATUS_INPROGRESS") {
                    //    Items = {
                            
                    //        "changesStatus": { name: "Change Status", icon: "fas fa-edit" },
                           
                    //    };
                    //}
                    return {
                        callback: function (key, options) {
                            switch (key) {
                                case 'items':
                                    onAddItem(id, serno);
                                    break;

                                case 'submit':
                                    OnSubmit(id);
                                    break;
                                case 'edit':
                                    onEdit(id);
                                    break;
                                case 'changesStatus':
                                  //  onEdit(id);
                                    break;

                                default:
                            }
                        },
                        items: Items
                    };


            }
        }
    });

    
});
function onEdit(id)
{
    var win = GetMainWindow();
    win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?id=' + id;
    win.OpenWindow({ Title: 'Edit Requisition', Width: 1000, Height: 800 });
    return false;
}
function onAddItem(id, serno) {
    if ($("#ItemHead").data("kendoDropDownList").value() == "" || $("#ItemHead").data("kendoDropDownList").value() == null || $("#ItemHead").data("kendoDropDownList").value() == undefined) {
        alert("Please select Item Head");
    }
    else {
        var win = GetMainWindow();
        win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisitionItems?serviceId=' + id + '&itemHead=' + $("#ItemHead").data("kendoDropDownList").value();
        win.OpenWindow({ Title: 'Requisition Detail-' + serno, Width: 1000, Height: 800 });
        return false;
    }
}
function getRequisitionData() {
    document.getElementById("RequisitionsOrderGrid").innerHTML = "";
    var sd = kendo.toString($("#RequisitionFromDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
    var ed = kendo.toString($("#RequisitionToDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
    gridConfig(
        "RequisitionsOrderGrid",
        "/IMS/InventoryManagement/ReadRequisitionData?ItemHead=" + $("#ItemHead").data("kendoDropDownList").value() + "&Customer=" + $("#Customer").data("kendoDropDownList").value() + "&Status=" + $("#RequisitionStatus").data("kendoDropDownList").value() + "&From=" + sd + "&To=" + ed,
        columnDefs,
        false,
        true,
        true,
        true,
        1,
        true,
        10);
}

function OnSearch(e) {

    getRequisitionData();
}

function AddRequisition()
{
    if ($("#ItemHead").data("kendoDropDownList").value() == "" || $("#ItemHead").data("kendoDropDownList").value() == null || $("#ItemHead").data("kendoDropDownList").value() == undefined) {
        alert("Please select Item Head");
    }
    else
    {
        var win = GetMainWindow();
        win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?itemHead=' + $("#ItemHead").data("kendoDropDownList").value();
        win.OpenWindow({ Title: 'New Requisition', Width: 1000, Height: 800 });
    }
    return false;
}
function OnSubmit(id) {
   
        $.ajax({
            url: '/IMS/InventoryManagement/CheckRequisitionItemsExist?requisitionId=' + id,
            type: 'POST',
            data: {},
            dataType: 'json',
            success: function (result) {
                debugger;
                if (result.success) {
                    var flag = confirm('Do you really want to Submit?');

                    if (flag) {
                        $.ajax({
                            url: '/IMS/InventoryManagement/SubmitRequisition?ServiceId=' + id,
                            type: 'POST',
                            data: {},
                            dataType: 'json',
                            success: function (result) {
                                debugger;
                                if (result.success) {
                                    getRequisitionData();
                                    kendo.alert("Submitted Successfully.");
                                } else {
                                    getRequisitionData();
                                    var err = result.errors.BinderCountError.errors[0];
                                    kendo.alert(err);
                                }
                            },
                            error: function (ert) {
                                getRequisitionData();
                            }
                        });
                        //return false;
                    }
                } else {                   
                    kendo.alert("Please Add Requisition Items First");
                }
            },
            error: function (ert) {
                getRequisitionData();
            }
        });


        
       
      
    
}
