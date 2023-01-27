var columnDefs = [
    //{
    //    name: "ProposalCode",
    //    title: "Proposal Code",
    //    type:"text"
    //},
    {
        name: "RequisitionCode",
        title: "Requisition Code",
        type: "text",
        
    },
    {
        name: "CreatedDate",
        title: "Schedule Date",
        type: "text"
    },
    {
        name: "AmountBase",
        title: "Amount Base Type",
        type: "text"
    },
   
];

$(function () {
    $("#CustomerId").kendoDropDownList({
        dataTextField: "CustomerName",
        dataValueField: "Id",
        optionLabel: "--Select--",

        filter: "contains",
        dataSource: {
            transport: {
                read: {
                    url: "/IMS/InventoryManagement/GetCustomerList",
                }
            }
        }
    });    
    $("#InvoiceGrid").jsGrid({
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
                //var sd = kendo.toString($("#FromDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
                //var ed = kendo.toString($("#ToDate").data("kendoDatePicker").value(), 'yyyy-MM-dd HH:mm:ss');
                return $.ajax({
                    type: "GET",
                    url: "/IMS/InventoryManagement/ReadScheduleInvoice?customerId=" + $("#CustomerId").data("kendoDropDownList").value(),
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
        
    });

  
    
});
function ScheduleInvoice(requisitionIds)
{
    var win = GetMainWindow();
    win.iframeOpenUrl = '/IMS/InventoryManagement/ScheduleInvoice?customerId=' + $("#CustomerId").data("kendoDropDownList").value() + '&requisitionIds=' + requisitionIds;
    win.OpenWindow({ Title: 'Invoice Schedule', Width: 1000, Height: 800 });
    return false;
}
function OnSearch(e) {
    debugger
    var grid = $("#InvoiceGrid").data("JSGrid");
    grid.render();
  
}



