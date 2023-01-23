var columnDefs = [
    {
        field: "ServiceNo",
        headerName: "Req. No"
    },
    {
        field: "RequisitionDate",
        headerName: "Req. Date"
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
        field: "WorkflowStatus",
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
            
            return "<div class='btn-group grid-menu' id='tree-menuBinUserRole' data-idvalue='" + params.value + "' data-status=\"" + params.data.ServiceStatusCode + "\" data-proposal=\"" + params.data.ProposalValue + "\" ><i class='fas fa-ellipsis-v'></i></div>"
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
    $("#RequisitionDate").kendoDatePicker({
       // value: new Date(),
        //min: new Date(),
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
    var RequisitionDate = $("#RequisitionDate").data("kendoDatePicker");   
    //RequisitionDate.readonly();
    var ItemHead = $("#ItemHead").data("kendoDropDownList");
    ItemHead.value($("#ItemHead").val());
    ItemHead.readonly();
    // getRequisitionData();
    
  
    if ($("#DataAction").val() =="Create") {
        
        RequisitionDate.value(new Date());
        RequisitionDate.min(new Date());
    }
    else
    {
        RequisitionDate.value($("#RequisitionDate").val());
       
    }

    $.contextMenu({
        selector: '#tree-menurequisition',
        trigger: 'left',
        build: function ($trigger, e) {


            var id = $trigger.data('idvalue');
            var proposalValue = $trigger.data('proposal');
            var status = $trigger.data('status');

            switch (0) {
                case 0:
                    //
                    var Items;
                    if (status == "SERVICE_STATUS_DRAFT") {
                        Items = {
                            "edit": { name: "Edit", icon: "fas fa-edit" },
                            "items": { name: "Item", icon: "fa-regular fa-cart-circle-plus" },
                            "submit": { name: "Submit", icon: "fa-regular fa-arrow-right-to-bracket" },
                        };
                    }
                    
                    return {
                        callback: function (key, options) {
                            switch (key) {
                                case 'items':
                                    onAddItem(id);
                                    break;

                                case 'submit':
                                    OnSubmit(id);
                                    break;
                                case 'edit':
                                    onEdit(id);
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

function OnSubmit(e) {
    var Customer = $("#Customer").data("kendoDropDownList").value();

    var RequisitionParticular = $("#RequisitionParticular").val();
    if (RequisitionParticular == null || RequisitionParticular == '' || RequisitionParticular == undefined) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("Requisition Particular is required");
        e.preventDefault();
        return false;
    }
    if (Customer == null || Customer == '' || Customer == undefined) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("Customer is required");
        e.preventDefault();
        return false;
    }



}
function onAjaxSuccess(res) {
    if (res.success) {
        closeNav1()
    }
    else {
        alert(res)
        showError(res.error);
    }
};
function closeNav1() {

    var win = GetMainWindow();
    win.CloseWindow({ MethodName: "OnSearch1" });
    return false;
}
function getRequisitionData() {
    document.getElementById("RequisitionsOrderGrid").innerHTML = "";
    gridConfig(
        "RequisitionsOrderGrid",
        "/IMS/InventoryManagement/ReadRequisitionData?ItemHead=" + $("#ItemHead").data("kendoDropDownList").value(),
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
function OnSubmit(e) {
    var Customer = $("#Customer").data("kendoDropDownList").value();

    var RequisitionParticular = $("#RequisitionParticular").val();
    if (RequisitionParticular == null || RequisitionParticular == '' || RequisitionParticular == undefined) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("Requisition Particular is required");
        e.preventDefault();
        return false;
    }
    if (Customer == null || Customer == '' || Customer == undefined) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("Customer is required");
        e.preventDefault();
        return false;
    }
}
function AddRequisition()
{
    var win = GetMainWindow();
    win.iframeOpenUrl = '/IMS/InventoryManagement/AddRequisition?itemHead=' + $("#ItemHead").data("kendoDropDownList").value();
    win.OpenWindow({ Title: 'Add New Requisition', Width: 1000, Height: 800 });
    return false;
}
