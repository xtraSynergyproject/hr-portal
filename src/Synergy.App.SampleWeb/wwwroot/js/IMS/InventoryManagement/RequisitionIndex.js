var columnDefs = [
    {
        field: "Id",
        headerName: "Select",
        cellRenderer: params => {
            return "<input type='checkbox' class='checkcontact' data-id='" + params.value + "'  data-itemhead='" + params.data.ItemHead + "' />"


        }
    },
    {
        field: "ServiceNo", cellRenderer: 'agGroupCellRenderer',
        headerName: "Req. No"
    },
    {
        field: "RequisitionDate",
        headerName: "Req. Date",
        cellRenderer: (data) => {
            var d = new Date(data.value);
            return d.getDate() + "." + d.getMonth()+1 + "." + d.getFullYear();
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
        field: "Id",
        cellRenderer: params => {
            
            return "<div class='btn-group grid-menu' id='tree-menurequisition' data-idvalue='" + params.value + "' data-status=\"" + params.data.ServiceStatusCode + "\" data-serviceno=\"" + params.data.ServiceNo + "\" ><i class='fas fa-ellipsis-v'></i></div>"
        }
    }
];


$(function () {
    $("#ItemHead").kendoDropDownList({
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
    $("#RequisitionFromDate").kendoDatePicker({
        value: d,
        format:"dd.MM.yyyy"
    });
    $("#RequisitionToDate").kendoDatePicker({
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
        change: function (e)
        {
            var dataItem = this.dataItem();
            if (dataItem.Name == "Complete") {
                $("#poRequest").show();
            }
            else
            {
                $("#poRequest").hide();
            }
        },
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
                    
                    var Items;
                    if (status == "SERVICE_STATUS_DRAFT") {
                        Items = {
                            "edit": { name: "Edit Requisition", icon: "fas fa-edit" },
                            "items": { name: "Item", icon: "fa-regular fa-cart-circle-plus" },
                            "submit": { name: "Submit", icon: "fa-regular fa-arrow-right-to-bracket" },
                        };
                    }
                    else if (status == "SERVICE_STATUS_COMPLETE"){
                        Items = {                            
                            "items": { name: "Item", icon: "fa-regular fa-cart-circle-plus" },                            
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
                                    onAddItem(id, serno, status);
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
    win.OpenWindow({ Title: 'Edit Requisition', Width: 700, Height: 800 });
    return false;
}
function onAddItem(id, serno,status) {
    if ($("#ItemHead").data("kendoDropDownList").value() == "" || $("#ItemHead").data("kendoDropDownList").value() == null || $("#ItemHead").data("kendoDropDownList").value() == undefined) {
        alert("Please select Item Head");
    }
    else {
        var win = GetMainWindow();
        win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisitionItems?RequisitionId=' + id + '&itemHead=' + $("#ItemHead").data("kendoDropDownList").value() + '&serStatus='+status;
        win.OpenWindow({ Title: 'Requisition Detail-' + serno, Width: 700, Height: 800 });
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

function OnSearch1() {
    debugger
    if ($("#ItemHead").data("kendoDropDownList").value() == "" || $("#ItemHead").data("kendoDropDownList").value() == null || $("#ItemHead").data("kendoDropDownList").value() == undefined) {
        ShowNotification("Please select Item Type","error");
    }
    else
    {
        getRequisitionData();
    }
   
}

function AddRequisition()
{
    if ($("#ItemHead").data("kendoDropDownList").value() == "" || $("#ItemHead").data("kendoDropDownList").value() == null || $("#ItemHead").data("kendoDropDownList").value() == undefined) {
        alert("Please select Item Type");
    }
    else
    {
        var win = GetMainWindow();
        win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?itemHead=' + $("#ItemHead").data("kendoDropDownList").value();
        win.OpenWindow({ Title: 'Add Requisition', Width: 700, Height: 800 });
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
                
                if (result.success) {
                    var flag = confirm('Do you really want to Submit?');

                    if (flag) {
                        ShowLoader($("#cms-content"));
                        $.ajax({
                            url: '/IMS/InventoryManagement/SubmitRequisition?requisitionId=' + id,
                            type: 'POST',
                            data: {},
                            dataType: 'json',
                            success: function (result) {
                                HideLoader($("#cms-content"));
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
                                HideLoader($("#cms-content"));
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
function AddPORequest()
{
    debugger  
    var allVals = [];
    $("input[class='checkcontact']:checked").each(function () {
        allVals.push($(this).attr("data-id"));
       
    });
    //alert(allVals.join(", "));
    //return;
    if (allVals.length > 0) { 

        $.ajax({
            url: '/IMS/InventoryManagement/ValidateForGeneratePORequest?requisitionIds=' + allVals.join(", "),
            type: 'GET',
            data: {},
            dataType: 'json',
            success: function (result) {
                
                if (result.success) {
                    var win = GetMainWindow();
                    win.iframeOpenUrl = '/IMS/InventoryManagement/POGeneration?requisitionIds=' + allVals.join(", ") + "&itemHeadId=" + $("#ItemHead").data("kendoDropDownList").value();
                    win.OpenWindow({ Title: 'PO Generation', Width: 700, Height: 800 });
                    
                } else {
                    ShowNotification("No items for Purchase order as all may have been added in PO.","error");
                }
            },
            error: function (ert) {
                kendo.alert(result.error);
            }
        });        
        
    }
    else {
        kendo.alert("Please Select the categories");
    }
    
}